var Enumeration = Enumeration || {};

//Giới tính
Enumeration.Gender = {
  Female: 1,
  Male: 0,
  Other: 2,
};

//Trạng thái làm việc
Enumeration.WorkStatus = {
  Active: 2,

  Retired: 1,
};
Enumeration.Status = {
  Active: 2,
  InAtice: 1,
};
Enumeration.TypeLaborContract = {
  TryWork: 3,
  Intern: 1,
  Office: 2,
};
//Các chế độ của form
Enumeration.FormMode = {
  Add: 1,
  Edit: 2,
  Delete: 3,
  Clone: 4,
};

//Loại lỗi input
Enumeration.ErrorType = {
  Require: 1,
  Unique: 2,
  MaxLength: 3,
  DataType: 4,
};

export default Enumeration;
