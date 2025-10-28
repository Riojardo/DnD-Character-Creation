using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using DnD_CharacterCreation.Data;
using Microsoft.AspNetCore.Routing.Template;

namespace DnD_CharacterCreation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PdfController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("fill/{characterId}")]
        public async Task<IActionResult> FillCharacterSheet(int characterId)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.CharacterId == characterId);
            var background = _context.Background.SingleOrDefault(b => b.BackgroundId == character.BackgroundId);
            if (character == null)
                return NotFound();

            var templatePath = "Content/pdf/5E_CharacterSheet_Fillable.pdf";

            using var tempStream = new MemoryStream();
            using (var reader = new PdfReader(templatePath))
            using (var writer = new PdfWriter(tempStream))
            using (var pdf = new PdfDocument(reader, writer))
            {
                var form = PdfAcroForm.GetAcroForm(pdf, true);
                form.GetField("CharacterName")?.SetValue(character.CharacterName);
                form.GetField("Alignment")?.SetValue(character.Alignment);
                form.GetField("ProfBonus")?.SetValue(character.ProficiencyBonus.ToString());
                form.GetField("Background")?.SetValue(background.Name);



                pdf.GetDocumentInfo().SetTitle(character.CharacterName);
            }

            var outputStream = new MemoryStream(tempStream.ToArray());
            outputStream.Position = 0;

            return File(outputStream, "application/pdf");
        }

    }
}

