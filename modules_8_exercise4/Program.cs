namespace modules_8_exercise4
{
    internal class Program
    {
       
            static void Main(string[] args)
            {
                string binaryFileName = @"C:\Users\Fed_w\Desktop\students.dat";
                string directoryPath = @"C:\Users\Fed_w\Desktop\Groups";

               
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                
                List<Student> students = ReadStudentsFromBinFile(binaryFileName);

                
                Dictionary<string, List<Student>> groupedStudents = GroupStudentsByGroup(students);
                foreach (var group in groupedStudents)
                {
                    string groupName = group.Key;
                    string filePath = Path.Combine(directoryPath, $"{groupName}.txt");
                    WriteStudentsToTextFile(group.Value, filePath);
                }

                Console.WriteLine("Данные успешно обработаны.");
            }

            static List<Student> ReadStudentsFromBinFile(string fileName)
            {
                List<Student> result = new List<Student>();

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        Student student = new Student();
                        student.Name = br.ReadString();
                        student.Group = br.ReadString();
                        long dt = br.ReadInt64();
                        student.DateOfBirth = DateTime.FromBinary(dt);
                        student.AverageScore = br.ReadDecimal();

                        result.Add(student);
                    }
                }

                return result;
            }

            static Dictionary<string, List<Student>> GroupStudentsByGroup(List<Student> students)
            {
                Dictionary<string, List<Student>> groupedStudents = new Dictionary<string, List<Student>>();

                foreach (var student in students)
                {
                    if (!groupedStudents.ContainsKey(student.Group))
                    {
                        groupedStudents[student.Group] = new List<Student>();
                    }
                    groupedStudents[student.Group].Add(student);
                }

                return groupedStudents;
            }

            static void WriteStudentsToTextFile(List<Student> students, string fileName)
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (var student in students)
                    {
                        writer.WriteLine($"{student.Name}, {student.DateOfBirth.ToShortDateString()}, {student.AverageScore}");
                    }
                }
            }
        
    }

       
        class Student
        {
            public string Name { get; set; }
            public string Group { get; set; }
            public DateTime DateOfBirth { get; set; }
            public decimal AverageScore { get; set; }
        }
    
}

