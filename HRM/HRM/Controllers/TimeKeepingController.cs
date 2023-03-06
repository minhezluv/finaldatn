using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using OpenCvSharp.Face;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeKeepingController : BasesController<TimeKeeping>
    {
        private readonly ITimeKeepingService JobService;
        private readonly ITimeKeepingRepository jobRepository;
        private readonly string _trainingDataPath;
        private LBPHFaceRecognizer _faceRecognizer;
        public TimeKeepingController(ITimeKeepingService mJobService, ITimeKeepingRepository mjobRepository) : base(mJobService)
        {
            JobService = mJobService;
            jobRepository = mjobRepository;
            _trainingDataPath = Path.Combine(AppContext.BaseDirectory, "trainingdata");
            Console.WriteLine(AppContext.BaseDirectory);
            _faceRecognizer = LBPHFaceRecognizer.Create();
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpGet("Filter")]
        public IActionResult GetByFilterPaging([FromQuery] string staffFilter, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string departmentID, [FromQuery] string positionName, [FromQuery] int Year , [FromQuery] int Month, [FromQuery] int Day)
        {
            try
            {
                var serviceResult = JobService.GetByFilterPaging(staffFilter, pageSize, pageIndex, departmentID, positionName, Year, Month, Day);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                     userMsg = "Lỗi",
                };

                return StatusCode(500, errObj);
            }
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpGet("InfoPage")]
        public IActionResult GetInfoPage([FromQuery] string staffFilter, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string departmentID, [FromQuery] string positionName, [FromQuery] int Year, [FromQuery] int Month, [FromQuery] int Day)
        {
            try
            {
                var serviceResult = JobService.GetInfoPage(staffFilter, pageSize, pageIndex, departmentID, positionName, Year, Month, Day);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "Lỗi",
                };

                return StatusCode(500, errObj);
            }
        }


        [EnableCors("AllowCROSPolicy")]
        [HttpGet("GetByMonth/{guid}")]
        public IActionResult GetByMonth(Guid guid)
        {
            try
            {
                var serviceResult = JobService.GetByMonth(guid);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "Lỗi",
                };

                return StatusCode(500, errObj);
            }
        }


        [EnableCors("AllowCROSPolicy")]
        [HttpGet("GetByWeekAll")]
        public IActionResult GetByWeekAll()
        {
            try
            {
                var serviceResult = JobService.GetByWeekAll();


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "Lỗi",
                };

                return StatusCode(500, errObj);
            }
        }


        [EnableCors("AllowCROSPolicy")]
        [HttpGet("GetByWeekOption")]
        public IActionResult GetByWeekOption(int year, int week)
        {
            try
            {
                var serviceResult = JobService.GetByWeekOption(year,week);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "Lỗi",
                };

                return StatusCode(500, errObj);
            }
        }


        [EnableCors("AllowCROSPolicy")]
        [HttpGet("GetByYearAll")]
        public IActionResult GetByYearAll(int year)
        {
            try
            {
                var serviceResult = JobService.GetByYearAll(year);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "Lỗi",
                };

                return StatusCode(500, errObj);
            }
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
                                serviceResult.data = result;
                                return StatusCode(200, serviceResult);
                            }

                            var staffCode = result.Replace(_trainingDataPath + "\\", "");
                            if (!staffCode.Contains("NV-"))
                            {
                                serviceResult.data = "unknow";
                                return StatusCode(200,serviceResult);
                            }
                          
                            Guid staffID = jobRepository.getIDbyStaffCode(staffCode.ToString());
                            if (staffID == Guid.Empty)
                            {
                                serviceResult.data = "Nhân viên đã nghỉ việc";
                                return StatusCode(200, serviceResult);
                            }
                            serviceResult.data = staffCode;
                            DateTime now = DateTime.Now;
                            List<FullTimeKeeping> tkLists = jobRepository.GetByFilterPaging(staffCode,10,1,"","",now.Year,now.Month,now.Day);
                           
                            if (tkLists.Count > 0)
                            {

                                return StatusCode(200,serviceResult);
                            }
                            TimeKeeping tk = new TimeKeeping();
                            tk.guid = Guid.NewGuid();
                            tk.StaffID = staffID;
                            tk.Start = DateTime.Today;
                            tk.End = DateTime.Today;
                            try
                            {
                                JobService.Insert(tk);
                            }
                            catch (Exception e)
                            {
                                return StatusCode(500, e);
                            }
                            
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

    }
}
