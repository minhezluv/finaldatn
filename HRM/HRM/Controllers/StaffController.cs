using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//using Emgu.CV;
//using Emgu.CV.Structure;
//using Emgu.CV.CvEnum;
//using System.Drawing;
//using Emgu.CV.Util;
using OpenCvSharp;
//using OpenCvSharp.Net;
using Mat = OpenCvSharp.Mat;
using CascadeClassifier = OpenCvSharp.CascadeClassifier;
using OpenCvSharp.Face;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : BasesController<Staff>
    {
        private readonly IStaffService StaffService;

        private readonly IStaffRepository staffRepository;
        private readonly string _trainingDataPath;
        private LBPHFaceRecognizer _faceRecognizer;
        public StaffController(IStaffService mStaffService, IStaffRepository mstaffRepository) : base(mStaffService)
        {
            StaffService = mStaffService;
            staffRepository = mstaffRepository;
            _trainingDataPath = Path.Combine(AppContext.BaseDirectory, "trainingdata");
            Console.WriteLine(AppContext.BaseDirectory);
            _faceRecognizer = LBPHFaceRecognizer.Create();
           // LoadTrainingData();



        }
        #region methods
        [HttpGet("Export")]
        public IActionResult Export(CancellationToken cancellationToken, [FromQuery] string filterValue)
        {
            var stream = StaffService.Export(cancellationToken, filterValue);

            string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
        [HttpPost("Import")]
        public IActionResult Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            CultureInfo culture = new CultureInfo("en-US");
            var staffs = new List<Staff>();
            try
            {

                using (var stream = new MemoryStream())

                {
                    formFile.CopyToAsync(stream, cancellationToken);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet excelWorksheet = package.Workbook.Worksheets[0];
                        var rowCount = excelWorksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var staff = new Staff
                            {
                                StaffCode = excelWorksheet.Cells[row, 2].Value.ToString().Trim(),
                                StaffName = excelWorksheet.Cells[row, 3].Value.ToString().Trim(),
                                DateOfBirth = Convert.ToDateTime(excelWorksheet.Cells[row, 4].Value.ToString().Trim(), culture),
                                Gender = Int32.Parse(excelWorksheet.Cells[row, 5].Value.ToString().Trim()),
                                HomeTown = excelWorksheet.Cells[row, 6].Value.ToString().Trim(),
                                //Ethnic = excelWorksheet.Cells[row, 7].Value.ToString().Trim(),
                                ////--//
                                //Religion= excelWorksheet.Cells[row, 8].Value.ToString().Trim(),
                                //Nationality= excelWorksheet.Cells[row, 9].Value.ToString().Trim(),
                                Address = excelWorksheet.Cells[row, 10].Value.ToString().Trim(),
                                PhoneNumber = excelWorksheet.Cells[row, 11].Value.ToString().Trim(),
                                Email = excelWorksheet.Cells[row, 12].Value.ToString().Trim(),
                                IdentityCard = excelWorksheet.Cells[row, 13].Value.ToString().Trim(),
                                IDCardPlace = excelWorksheet.Cells[row, 14].Value.ToString().Trim(),
                                //StartDate = Convert.ToDateTime(excelWorksheet.Cells[row, 15].Value.ToString().Trim(), culture) ,
                                //EndDate = Convert.ToDateTime(excelWorksheet.Cells[row, 16].Value.ToString().Trim(), culture),
                                //Note= excelWorksheet.Cells[row, 17].Value.ToString().Trim()
                            };
                            staffs.Add(staff);
                        }
                        int response = staffRepository.Import(staffs);
                    }

                    // ExcelWorksheet ws = package.Workbook.
                }
            }
            catch (Exception)
            {

                throw;
            }
            return StatusCode(200, response);
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpGet("Filter")]
        public IActionResult GetByFilterPaging([FromQuery] string employeeFilter, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string departmentID, [FromQuery] string positionID,int status)
        {
            try
            {
                var serviceResult = StaffService.GetByFilterPaging(employeeFilter, pageSize, pageIndex, departmentID, positionID,status);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                   
                };

                return StatusCode(500, errObj);
            }
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpGet("InfoPage")]
        public IActionResult GetInfoPage([FromQuery] string employeeFilter, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string departmentID, [FromQuery] string positionID,int status)
        {
            try
            {
                var serviceResult = StaffService.GetInfoPage(employeeFilter, pageSize, pageIndex, departmentID, positionID,status);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    // userMsg = MISA.Test.Core.Resources.ErrorMsg.ServerError_ErrorMsg,
                };

                return StatusCode(500, errObj);
            }

        }

        [EnableCors("AllowCROSPolicy")]
        [HttpGet("GetAllStaffPerMonth")]
        public IActionResult GetAllStaffPerMonth(int year)
        {
            try
            {
                var serviceResult = StaffService.GetAllStaffPerMonth(year);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    // userMsg = MISA.Test.Core.Resources.ErrorMsg.ServerError_ErrorMsg,
                };

                return StatusCode(500, errObj);
            }

        }

        [HttpGet("LastStaffCode")]
        public IActionResult GetLastStaffCode()
        {
            try
            {
                var serviceResult = this.StaffService.GetLastStaffCode();

                return StatusCode(200, serviceResult.data);
            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "không thể kết nối",
                };

                return StatusCode(500, errObj);
            }
        }

        [HttpGet("CheckStaffCodeExists/{employeeCode}/{staffID}")]
        public IActionResult CheckEmployeeCodeExists(string employeeCode, string staffID)
        {
            try
            {
                var serviceResult = this.StaffService.CheckEmployeeCodeExists(employeeCode, staffID);
                if (!serviceResult.success)
                {
                    var errObj = new
                    {
                        devMsg = serviceResult.devMsg,
                        userMsg = serviceResult.userMsg,
                    };

                    return BadRequest(errObj);
                }

                return StatusCode(200, serviceResult.data);
            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "không thể kết nối server",
                };

                return StatusCode(500, errObj);
            }
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpPost("InsertStaff")]

        public IActionResult InsertStaff(Staff staff)
        {
            try
            {
                var serviceResult = this.StaffService.InsertStaff(staff);
                if (!serviceResult.success)
                {
                    var errObj = new
                    {
                        devMsg = serviceResult.devMsg,
                        userMsg = serviceResult.userMsg,
                    };

                    return BadRequest(errObj);
                }
                return StatusCode(200, serviceResult);
            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "không thể kết nối server",
                };

                return StatusCode(500, errObj);
            }
        }
        //[EnableCors("AllowCROSPolicy")]
        //[HttpPost("CheckFace")]

        //public  IActionResult checkFaceAsync(IFormFile file)
        //{


        //    {
        //        if (file == null || file.Length == 0)
        //        {
        //            return BadRequest();
        //        }

        //        using (var memoryStream = new MemoryStream())
        //        {

        //            string startup = "E:/20221/HRM/HRM/bin/Debug";
        //            file.CopyToAsync(memoryStream);
        //            using (var img = Image.FromStream(memoryStream))
        //            {
        //                // TODO: ResizeImage(img, 100, 100);
        //                //  img = img.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC)

        //                // var image = new Image<Bgr, byte>(img);
        //                Bitmap bmpImage = new Bitmap(img);
        //                var newimg = new Image<Gray, Byte>(bmpImage);
        //                newimg = newimg.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
        //               // response = StaffService.GetAll();

        //                List<Staff> staffs = staffRepository.GetAll();
        //                List<Guid> labelGuid = new List<Guid>() ;
        //                List<String> labelCode = new List<string >();
        //                //foreach(Staff staff in staffs)
        //                //{
        //                //    labelGuid.Add(staff.guid);
        //                //    labelCode.Add(staff.StaffCode);
        //                //}
        //                int size = staffs.Count();
        //                string LoadFaces;
        //                List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        //                foreach (Staff staff in staffs)
        //                {
        //                    LoadFaces = staff.guid + ".bmp";
        //                    trainingImages.Add(new Image<Gray, byte>(startup+ "/TrainedFaces/" + LoadFaces));
        //                    labelGuid.Add(staff.guid);
        //                    labelCode.Add(staff.StaffCode);

        //                }

        //                if (trainingImages.ToArray().Length != 0)
        //                {
        //                    //TermCriteria for face recognition with numbers of trained images like maxIteration
        //                    MCvTermCriteria termCrit = new MCvTermCriteria(size, 0.001);

        //                    //Eigen face recognizer
        //                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
        //                       trainingImages.ToArray(),
        //                       labelCode.ToArray(),
        //                       3000,
        //                       ref termCrit);

        //                   var name = recognizer.Recognize(newimg);

        //                    //Draw the label for each face detected and recognized
        //                   // currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

        //                }
        //            }
        //        }
        //    }
        //    return BadRequest();
        //}


        //[HttpPost]
        //public int Post(Mat face)
        //{
        //    using var model = new EigenFaceRecognizer(1,00.1);
        //    model.Train(
        //        new List<Mat> { face }, new List<int> { 1 });
        //    return model.Predict((IImage)face).Label;
        //}
        [EnableCors("AllowCROSPolicy")]
        [HttpPost("detectFace")]
        public async Task<IActionResult> DetectFaces(IFormFile image)
        {
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            Mat imageMat = Cv2.ImDecode(imageBytes, ImreadModes.Color);
            Mat grayImage = new Mat();
            Cv2.CvtColor(imageMat, grayImage, ColorConversionCodes.BGR2GRAY);
            OpenCvSharp.CascadeClassifier faceClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
            //      Rect[] faces = faceClassifier.DetectMultiScale(grayImage, 1.1, 3, HaarDetectionType.ScaleImage, new Size(20, 20));
            Rect[] faces = faceClassifier.DetectMultiScale(grayImage, 1.1, 5, (HaarDetectionTypes)0, new OpenCvSharp.Size(50, 50));
            //var face = faces.Last();
            //SortedList(faces,)
            foreach (Rect face in faces)
            {
                Cv2.Rectangle(imageMat, face, Scalar.Red, 2);
            }

            // Cv2.Rectangle(imageMat, face, Scalar.Red, 2);

            return File(imageMat.ToBytes(".jpg"), "image/jpeg");
        }


        [EnableCors("AllowCROSPolicy")]
        [HttpPost("ReFace")]
        public async Task<IActionResult> Recognize(IFormFile image)

        {
            try
            {
                _faceRecognizer.Read("trainedModel.xml");
                if (image == null)
                {
                    return BadRequest("Image is required.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    var serviceResult = new ServiceResult();
                    //LoadTrainingData();
                    image.CopyTo(memoryStream);
                    var imageData = memoryStream.ToArray();
                    using (var mat = Cv2.ImDecode(imageData, ImreadModes.Grayscale))
                    {
                        OpenCvSharp.CascadeClassifier faceClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
                        //      Rect[] faces = faceClassifier.DetectMultiScale(grayImage, 1.1, 3, HaarDetectionType.ScaleImage, new Size(20, 20));
                        //Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2GRAY);
                        Rect[] faces = faceClassifier.DetectMultiScale(mat, 1.1, 5, (HaarDetectionTypes)1, new OpenCvSharp.Size(100, 100));
                        var face = faces.First();

                        //Mat srcContinous = new Mat();
                        //mat.Clone().ConvertTo(srcContinous, MatType.CV_32FC1);
                        var faceMat = new Mat(mat, face);
                        OpenCvSharp.Size size = new OpenCvSharp.Size(100, 100);
                        Mat resizedImg = new Mat();
                        OpenCvSharp.Cv2.Resize(faceMat, resizedImg, size);
                        faceMat = resizedImg;


                        using (faceMat)
                        {
                            //_faceRecognizer = EigenFaceRecognizer.Create();
                            //LoadTrainingData();
                            var images = Directory.GetFiles(_trainingDataPath, "*.jpg", SearchOption.AllDirectories);
                            var Folder = Directory.GetDirectories(_trainingDataPath);
                            int label;
                            string result;
                            double confidence;
                            //   LoadTrainingData();
                            if (!faceMat.IsContinuous())
                            {
                                Mat temp = new Mat();
                                faceMat.CopyTo(temp);
                                faceMat = temp;

                                _faceRecognizer.Predict(faceMat, out label, out confidence);
                            }
                            else
                            {
                                _faceRecognizer.Predict(faceMat, out label, out confidence);
                            }


                            if (label >= 0 && confidence >= 0.5)
                            {
                                //result = Path.GetFileNameWithoutExtension(images[label]);
                                //result = Folder[label];
                                result = Folder[label];
                            }
                            else
                            {
                                result = "not found";
                            }

                            serviceResult.data = result.Replace(_trainingDataPath + "\\", "");
                            serviceResult.moreInfo = confidence.ToString();

                            return Ok(serviceResult);
                            //return File(faceMat.ToBytes(".jpg"), "image/jpeg");
                        }
                    }
                }
                }
            catch
            {
                return BadRequest();
            }
          
            
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpPost("UpFace")]
        public IActionResult UpFaces(List<IFormFile> images, string code)
        {
            //// Load the input image
            //Mat image = Cv2.ImRead(
            //    file.OpenReadStream(), ImreadModes.Color);

            //OpenCvSharp.CascadeClassifier faceClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
            ////      Rect[] faces = faceClassifier.DetectMultiScale(grayImage, 1.1, 3, HaarDetectionType.ScaleImage, new Size(20, 20));
            //Rect[] faces = faceClassifier.DetectMultiScale(mat, 1.1, 3, (HaarDetectionTypes)1, new OpenCvSharp.Size(20, 20));
            //var face = faces.First();
            //// Save the faces to disk
            //int faceIndex = 0;
            //foreach (Rect face in faces)
            //{
            //    Mat faceImage = new Mat(image, face);
            //    Cv2.ImWrite($"face_{faceIndex}.jpg", faceImage);
            //    faceIndex++;
            //}

            //return Ok();
            for(int i = 0; i< images.Count(); i++)

            {
                IFormFile image = images[i];
                if (image == null)
                {
                    return BadRequest("Image is required.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    //var imgpath= Path.Combine(Ba, code);

                    //var subimgpath = Path.Combine(imgpath, num);

                    var path = Path.Combine(_trainingDataPath, code);
                    if (!Directory.Exists(path))
                    {
                        // Create the directory
                        Directory.CreateDirectory(path);
                    }
                    var subimgpath = Path.Combine(path,(i+1).ToString());
                    //LoadTrainingData();
                    image.CopyTo(memoryStream);
                    var imageData = memoryStream.ToArray();
                    using (var mat = Cv2.ImDecode(imageData, ImreadModes.Grayscale))
                    {

                        OpenCvSharp.CascadeClassifier faceClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
                        //      Rect[] faces = faceClassifier.DetectMultiScale(grayImage, 1.1, 3, HaarDetectionType.ScaleImage, new Size(20, 20));
                        Rect[] faces = faceClassifier.DetectMultiScale(mat, 1.1, 3, (HaarDetectionTypes)1, new OpenCvSharp.Size(100, 100));
                        var face = faces.First();

                        //Mat srcContinous = new Mat();
                        //mat.Clone().ConvertTo(srcContinous, MatType.CV_32FC1);
                        //// mat = srcContinous;
                        //Mat faceImage = new Mat(srcContinous, face);
                        Mat faceImage = new Mat(mat, face);
                        OpenCvSharp.Size size = new OpenCvSharp.Size(100, 100);
                        Mat resizedImg = new Mat();
                        OpenCvSharp.Cv2.Resize(faceImage, resizedImg, size);
                        faceImage = resizedImg;

                        if (!faceImage.IsContinuous())
                        {
                            Mat temp = new Mat();
                            faceImage.CopyTo(temp);
                            faceImage = temp;
                        }
                        Cv2.ImWrite(subimgpath + ".jpg", faceImage);
                       
                    }
                }
                }

            LoadTrainingData();
            return Ok(code);
        }





        #endregion


        #region adding methods
        private void LoadTrainingData()
        {
            if (!Directory.Exists(_trainingDataPath))
            {
                Directory.CreateDirectory(_trainingDataPath);
            }

            //var images = Directory.GetFiles(_trainingDataPath, "*.jpg", SearchOption.AllDirectories);
            //if (!images.Any())
            //{
            //    return;
            //}

            //var faceImages = new Mat[images.Length];
            //var faceLabels = new string[images.Length];
            //var faceLabelsInput = new int[images.Length];
            //for (var i = 0; i < images.Length; i++)
            //{
            //    faceImages[i] = Cv2.ImRead(images[i]);
            //    faceLabels[i] = (Path.GetDirectoryName(images[i]));
            //    faceLabelsInput[i] = i;
            //}

            //_faceRecognizer.Train(faceImages, faceLabelsInput);

            var images = Directory.GetFiles(_trainingDataPath, "*.jpg", SearchOption.AllDirectories);
            var Folder = Directory.GetDirectories(_trainingDataPath);
            if (!images.Any())
            {
                return;
            }
            var faceImages = new Mat[images.Length];
            var faceLabels = new int[images.Length];

            int cnt = 0;
            for (var i = 0; i < Folder.Length; i++)
            {
                var subpath = Path.Combine(_trainingDataPath, Folder[i]);
                var subimg = Directory.GetFiles(subpath, "*.jpg", SearchOption.AllDirectories);
                for (var j = 0; j < subimg.Length; j++)
                {
                    faceImages[cnt] = Cv2.ImRead(images[cnt]);
                    faceLabels[cnt] = i ;
                    Cv2.CvtColor(faceImages[cnt], faceImages[cnt], ColorConversionCodes.BGR2GRAY);
                    cnt++;
                }
            }

            _faceRecognizer.Train(faceImages, faceLabels);
            _faceRecognizer.Save("trainedModel.xml");
            //var dd= _faceRecognizer.GetLabelsByString();
            //var sdas = "00";
        }






        #endregion

    }

}
