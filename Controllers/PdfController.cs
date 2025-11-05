using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using DnD_CharacterCreation.Data;
using Microsoft.AspNetCore.Routing.Template;
using DnD_CharacterCreation.Models;

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
            
            var classFirst = await _context.CharacterClass
                            .Where(cc => cc.CharacterId == characterId)
                            .OrderBy(cc => cc.CharacterClassId)   
                            .FirstOrDefaultAsync();
            var classNameFirst = await _context.Class.FirstOrDefaultAsync(c => c.ClassId == classFirst.ClassId);


            var classSecond = await _context.CharacterClass
                           .Where(cc => cc.CharacterId == characterId)
                           .OrderBy(cc => cc.CharacterClassId)
                           .Skip(1)
                           .FirstOrDefaultAsync();
            var classNameSecond = await _context.Class.FirstOrDefaultAsync(c => c.ClassId == classSecond.ClassId);


            var classThird= await _context.CharacterClass
                           .Where(cc => cc.CharacterId == characterId)
                           .OrderBy(cc => cc.CharacterClassId)
                           .Skip(2)
                           .FirstOrDefaultAsync();
             var classNameThird = await _context.Class.FirstOrDefaultAsync(c => c.ClassId == classThird.ClassId);




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
                form.GetField("ClassLevel")?.SetValue($"{classNameFirst.Name}/{classFirst.Level} {classNameSecond.Name}/{classSecond.Level} {classNameThird.Name}/{classThird.Level}");
               

                pdf.GetDocumentInfo().SetTitle(character.CharacterName);
            }

            var outputStream = new MemoryStream(tempStream.ToArray());
            outputStream.Position = 0;

            return File(outputStream, "application/pdf");
        }

    }
}

