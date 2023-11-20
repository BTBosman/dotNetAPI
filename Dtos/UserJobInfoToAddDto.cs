namespace DotnetAPI.Models
{
    public partial class UserJobInfoToAddDto
    {
        public int UserId {get; set;}
        public string JobTitle {get; set;}
        public string Department {get; set;}

        public UserJobInfoToAddDto()
        {
            if(JobTitle == null) 
            {
                JobTitle = "";
            }
            if(Department == null)
            {
                Department = "";
            }
        }
    }
}