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
            var specie = _context.Race.SingleOrDefault(b => b.RaceId == character.RaceId);
            var subspecie = _context.SubRace.SingleOrDefault(b => b.RaceId == character.SubRaceId);

            var characterClasses = await _context.CharacterClass
                                  .Where(cc => cc.CharacterId == characterId)
                                  .OrderBy(cc => cc.CharacterClassId)
                                  .ToListAsync();

                                   var classInfos = new List<string>();
                                   int levelTotal = new int();

                                   foreach (var cc in characterClasses)
                                   {
                                        var classEntity = await _context.Class.FirstOrDefaultAsync(c => c.ClassId == cc.ClassId);
                                        if (classEntity != null)
                                            classInfos.Add($"{classEntity.Name}/{cc.Level}");
                                            levelTotal = cc.Level + levelTotal;
                                   }



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
                form.GetField("ClassLevel")?.SetValue(string.Join(" ", classInfos));
                form.GetField("Background")?.SetValue(background.Name);
                form.GetField("Race ")?.SetValue($"{specie.RaceName}");
                form.GetField("Alignment")?.SetValue(character.Alignment);
                form.GetField("ProfBonus")?.SetValue((1 + (int)Math.Ceiling(levelTotal / 4.0)).ToString());
                
                
                


                pdf.GetDocumentInfo().SetTitle(character.CharacterName).SetSubject(character.CharacterName);
            }

            var outputStream = new MemoryStream(tempStream.ToArray());
            outputStream.Position = 0;

            return File(outputStream, "application/pdf");
        }

    }
}

