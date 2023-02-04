namespace Clinic.Models
{
    
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }


        public Client(string name, string surname, int age, string gender, string email)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Surname = surname;
            Age = age;
            Gender = gender;
            Email = email;
        }

        public bool IsClientNeeded(string name, string surname)
        {
            if(Name == name && Surname == surname) return true;
            return false;
        }
        public bool IsClientNeeded(string id) => Id == id ? true : false;
        
    }
}
