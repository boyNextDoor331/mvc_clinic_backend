namespace Clinic.Models
{
    
    public class Client
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }


        public Client(string name, string surname, int age, string gender, string email)
        {
            Name = name;
            Surname = surname;
            Age = age;
            Gender = gender;
            Email = email;
        }
    }
}
