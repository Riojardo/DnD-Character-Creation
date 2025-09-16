using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;


namespace DnD_CharacterCreation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        [HttpPost("fill")]
        public IActionResult FillCharacterSheet([FromBody] CharacterDto character)
        {
            // Load template
            using var reader = new PdfReader("Content/5E_CharacterSheet_Fillable.pdf");
            using var ms = new MemoryStream();
            using var pdf = new PdfDocument(reader, new PdfWriter(ms));

            var form = PdfAcroForm.GetAcroForm(pdf, true);

            // Fill fields
            form.GetField("CharacterName").SetValue(character.Name);
            form.GetField("ClassLevel").SetValue(character.ClassLevel);
            form.GetField("Race").SetValue(character.Race);
            form.GetField("STR").SetValue(character.Strength.ToString());

            form.FlattenFields(); // makes fields uneditable
            pdf.Close();

            // Return PDF to browser
            return File(ms.ToArray(), "application/pdf", $"{character.Name}_Sheet.pdf");
        }
    }
    public class CharacterDto
    {
        public string Name { get; set; } = "Toby";
        public string ClassLevel { get; set; } = "18";
        public string Race { get; set; } = "orc";
        public int Strength { get; set; } = 12;
    }
}



