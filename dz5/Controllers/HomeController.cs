using dz5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace dz5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly FileService _fileService;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, FileService fileService)
        {
            _logger = logger;
            _environment = environment;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Test()
        {
            Contact contact = new Contact
            {
                Name = "John Doe",
                MobilePhone = "123-456-7890",
                AlternativeMobilePhone = "987-654-3210",
                Email = "john.doe@example.com",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            return View(contact);
        }

        public ActionResult Show(Contact contact)
        {
            SaveToFile(contact, "contacts.xml");
            return View("Test", contact);
        }

        public IActionResult ShowContacts()
        {
            return View();
        }

        public IActionResult LoadFromFile()
        {
            List<Contact> contacts = LoadContactsFromFile("contacts.xml");
            if (contacts == null)
            {
                ViewBag.Message = "Failed to load contacts.";
                return View("ShowContacts", new List<Contact>());
            }
            ViewBag.Message = "Contacts loaded successfully.";
            return View("ShowContacts", contacts);
        }

        private List<Contact> LoadContactsFromFile(string filePath)
        {
            try
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string targetFilePath = Path.Combine(uploadsFolder, filePath);

                System.IO.File.Copy(filePath, targetFilePath, true);

                XmlSerializer serializer = new XmlSerializer(typeof(List<Contact>));

                using (StreamReader reader = new StreamReader(targetFilePath))
                {
                    List<Contact> contacts = (List<Contact>)serializer.Deserialize(reader);
                    return contacts;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading contacts from file: {ex.Message}");
                return null;
            }
        }

        private void SaveToFile(Contact contact, string filePath)
        {
            try
            {
                List<Contact> contacts = LoadContactsFromFile(filePath) ?? new List<Contact>();
                contacts.Add(contact);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Contact>));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, contacts);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while saving contact to file: {ex.Message}");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
