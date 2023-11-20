namespace DotnetAPI.Dtos
{
    public partial class UserToAddDto
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public bool Active {get; set;}
        public string Gender {get; set;}
        public string Email {get;set;}

        public UserToAddDto() 
        {
            if(FirstName == null) 
            {
                FirstName = "";
            }

            if(LastName == null) 
            {
                LastName = "";
            }

            if(Gender == null) 
            {
                Gender = "";
            }

            if(Email == null) 
            {
                Email = "";
            }
        }
    }
}