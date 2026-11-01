namespace BusinessLayer;

/// <summary>Static reference data the <see cref="DataSeeder"/> builds the college from.</summary>
internal static class SeedData
{
    public record Programme(
        string Name,
        string Code,
        string Description,
        DateTime Established,
        int IntakeSeats,
        double FemaleShare,
        string[][] Curriculum);

    public static readonly string[] SemesterNames =
        ["Semester I", "Semester II", "Semester III", "Semester IV", "Semester V", "Semester VI", "Semester VII", "Semester VIII"];

    /// <summary>First-year courses shared by every BE programme, taught by Science &amp; Humanities.</summary>
    public static readonly string[][] CommonEngineeringYear =
    [
        ["Engineering Mathematics I", "Engineering Physics", "Computer Programming", "Engineering Drawing I", "Basic Electrical Engineering", "Workshop Technology"],
        ["Engineering Mathematics II", "Engineering Chemistry", "Applied Mechanics", "Engineering Drawing II", "Basic Electronics Engineering", "Thermal Science"],
    ];

    public static readonly Programme Computer = new(
        "Computer Engineering", "BCT",
        "Bachelor in Computer Engineering: software, hardware, networks and intelligent systems.",
        new DateTime(2000, 7, 16), 48, 0.24,
        [
            [], [],
            ["Object Oriented Programming", "Discrete Structures", "Electric Circuit Theory", "Electronic Devices and Circuits", "Digital Logic", "Engineering Mathematics III"],
            ["Data Structures and Algorithms", "Microprocessors", "Computer Graphics", "Theory of Computation", "Probability and Statistics", "Instrumentation I"],
            ["Computer Organization and Architecture", "Software Engineering", "Database Management Systems", "Operating Systems", "Numerical Methods", "Communication English"],
            ["Computer Networks", "Artificial Intelligence", "Embedded Systems", "Object Oriented Analysis and Design", "Simulation and Modelling", "Minor Project"],
            ["Distributed Systems", "Digital Signal Analysis and Processing", "Information Systems", "Engineering Economics", "Elective I: Machine Learning", "Major Project I"],
            ["Information Security", "Big Data Technologies", "Organization and Management", "Engineering Professional Practice", "Elective II: Cloud Computing", "Major Project II"],
        ]);

    public static readonly Programme Civil = new(
        "Civil Engineering", "BCE",
        "Bachelor in Civil Engineering: structures, geotechnics, water resources and transportation.",
        new DateTime(2000, 7, 16), 48, 0.22,
        [
            [], [],
            ["Engineering Mathematics III", "Strength of Materials", "Fluid Mechanics", "Surveying I", "Engineering Geology", "Building Materials"],
            ["Probability and Statistics", "Theory of Structures I", "Hydraulics", "Surveying II", "Soil Mechanics", "Building Drawing"],
            ["Theory of Structures II", "Concrete Technology and Masonry", "Water Supply Engineering", "Engineering Hydrology", "Transportation Engineering I", "Numerical Methods"],
            ["Design of Steel and Timber Structures", "Foundation Engineering", "Transportation Engineering II", "Sanitary Engineering", "Irrigation and Drainage", "Survey Camp"],
            ["Design of RCC Structures", "Construction Management", "Hydropower Engineering", "Engineering Economics", "Elective I: Earthquake Engineering", "Major Project I"],
            ["Estimating and Valuation", "Bridge Engineering", "Organization and Management", "Engineering Professional Practice", "Elective II: Disaster Risk Management", "Major Project II"],
        ]);

    public static readonly Programme Electrical = new(
        "Electrical Engineering", "BEL",
        "Bachelor in Electrical Engineering: power systems, machines, control and power electronics.",
        new DateTime(2003, 8, 17), 36, 0.17,
        [
            [], [],
            ["Engineering Mathematics III", "Electric Circuit Theory", "Electronic Devices and Circuits", "Digital Logic", "Electromagnetics", "Electrical Engineering Materials"],
            ["Electrical Machines I", "Probability and Statistics", "Microprocessors", "Instrumentation I", "Network Theory", "Electrical Measurements"],
            ["Electrical Machines II", "Power Electronics", "Control Systems", "Numerical Methods", "Instrumentation II", "Communication English"],
            ["Power System Analysis I", "Utilization of Electrical Energy", "Digital Control Systems", "Switchgear and Protection", "Hydropower Engineering", "Minor Project"],
            ["Power System Analysis II", "High Voltage Engineering", "Electrical Machine Design", "Engineering Economics", "Elective I: Smart Grid", "Major Project I"],
            ["Power Plant Design", "Renewable Energy Technology", "Organization and Management", "Engineering Professional Practice", "Elective II: Electric Vehicles", "Major Project II"],
        ]);

    public static readonly Programme InformationTechnology = new(
        "BSc. IT", "BIT",
        "Bachelor of Science in Information Technology, open to students from any intermediate stream.",
        new DateTime(2008, 1, 13), 40, 0.31,
        [
            ["Introduction to Information Technology", "C Programming", "Digital Logic", "Mathematics I", "Physics"],
            ["Discrete Structures", "Object Oriented Programming", "Microprocessor", "Mathematics II", "Statistics I"],
            ["Data Structures and Algorithms", "Numerical Methods", "Computer Architecture", "Computer Graphics", "Statistics II"],
            ["Theory of Computation", "Computer Networks", "Operating Systems", "Database Management Systems", "Artificial Intelligence"],
            ["Design and Analysis of Algorithms", "System Analysis and Design", "Cryptography", "Simulation and Modelling", "Web Technology"],
            ["Software Engineering", "Compiler Design and Construction", "E-Governance", "NET Centric Computing", "Technical Writing"],
            ["Advanced Java Programming", "Data Warehousing and Data Mining", "Principles of Management", "Cloud Computing", "Project Work"],
            ["Advanced Database", "Mobile Application Development", "Information Retrieval", "Network Security", "Internship"],
        ]);

    public static readonly Programme[] Programmes = [Computer, Civil, Electrical, InformationTechnology];

    public static readonly string[] MaleNames =
    [
        "Aarav", "Abhishek", "Aashish", "Anish", "Anil", "Anup", "Arjun", "Ayush", "Bibek", "Bikash", "Binod", "Bishal",
        "Deepak", "Dipesh", "Gaurav", "Hari", "Kiran", "Kishor", "Krishna", "Kushal", "Manish", "Milan", "Nabin", "Niraj",
        "Nischal", "Pawan", "Prabin", "Prakash", "Pramod", "Prashant", "Rabin", "Rajan", "Rajesh", "Ramesh", "Rohan", "Roshan",
        "Sagar", "Sajan", "Samir", "Sandeep", "Sanjay", "Santosh", "Saroj", "Shishir", "Sudip", "Sujan", "Sunil", "Suraj",
        "Suresh", "Sushant", "Tilak", "Ujjwal", "Umesh", "Yogesh", "Aakash", "Bijay", "Dinesh", "Ishan", "Pratik", "Subash",
    ];

    public static readonly string[] FemaleNames =
    [
        "Aakriti", "Aarati", "Anjali", "Anisha", "Asmita", "Bandana", "Barsha", "Bipana", "Deepa", "Elina", "Gita", "Jyoti",
        "Kabita", "Karishma", "Kritika", "Laxmi", "Manisha", "Menuka", "Nirmala", "Nisha", "Pooja", "Prabina", "Pragya", "Pratibha",
        "Prerana", "Priya", "Rachana", "Rasmita", "Rekha", "Riya", "Sabina", "Sadikshya", "Samjhana", "Sangita", "Sarita", "Shristi",
        "Sita", "Smriti", "Sneha", "Srijana", "Sujata", "Sunita", "Susmita", "Swastika", "Trishna", "Usha",
    ];

    public static readonly string[] Surnames =
    [
        "Acharya", "Adhikari", "Aryal", "Basnet", "Bhandari", "Bhattarai", "Bista", "Chaudhary", "Dahal", "Dhakal", "Ghimire",
        "Gurung", "Joshi", "Karki", "KC", "Khadka", "Koirala", "Lama", "Magar", "Maharjan", "Neupane", "Oli", "Pandey",
        "Pant", "Paudel", "Pokharel", "Pradhan", "Rai", "Rana", "Regmi", "Sapkota", "Shah", "Shakya", "Sharma", "Shrestha",
        "Subedi", "Tamang", "Thapa", "Timilsina", "Tiwari", "Upreti", "Wagle", "Yadav", "Bajracharya", "Limbu", "Sherpa",
    ];

    public static readonly string[] MaleMiddleNames = ["Bahadur", "Prasad", "Kumar", "Raj", "Man"];
    public static readonly string[] FemaleMiddleNames = ["Kumari", "Maya", "Devi"];

    public static readonly string[] Localities =
    [
        "Baneshwor, Kathmandu", "Koteshwor, Kathmandu", "Kalanki, Kathmandu", "Chabahil, Kathmandu", "Baluwatar, Kathmandu",
        "Maharajgunj, Kathmandu", "Budhanilkantha, Kathmandu", "Kirtipur, Kathmandu", "Kupondole, Lalitpur", "Jawalakhel, Lalitpur",
        "Satdobato, Lalitpur", "Imadol, Lalitpur", "Suryabinayak, Bhaktapur", "Thimi, Bhaktapur", "Lakeside, Pokhara",
        "Bharatpur, Chitwan", "Butwal, Rupandehi", "Itahari, Sunsari", "Biratnagar, Morang", "Hetauda, Makwanpur",
        "Dhangadhi, Kailali", "Nepalgunj, Banke", "Birgunj, Parsa", "Janakpur, Dhanusha", "Damak, Jhapa",
    ];

    /// <summary>Teaching posts per engineering department, from most to least senior.</summary>
    public static readonly (string Position, int Count)[] DepartmentStaffing =
    [
        ("Professor", 1), ("Associate Professor", 2), ("Assistant Professor", 4), ("Lecturer", 5),
        ("Teaching Assistant", 2), ("Lab Instructor", 2), ("Department Secretary", 1),
    ];

    public static readonly (string Position, int Count)[] ScienceStaffing =
    [
        ("Associate Professor", 2), ("Assistant Professor", 3), ("Lecturer", 6), ("Lab Instructor", 2),
    ];

    public static readonly (string Position, int Count)[] AdministrationStaffing =
    [
        ("Principal", 1), ("Vice Principal", 1), ("Registrar", 1), ("Examination Controller", 1), ("Finance Officer", 1),
        ("Accountant", 2), ("Librarian", 1), ("Assistant Librarian", 1), ("Admission Officer", 1), ("IT Officer", 2),
        ("Receptionist", 1), ("Office Assistant", 3),
    ];
}
