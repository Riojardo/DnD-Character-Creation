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
            var subspecie = await _context.SubRace.FirstOrDefaultAsync(b => b.RaceId == character.SubRaceId);

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
                                   var totalXP = levelTotal switch
                                   {
                                       <= 1 => 0,
                                       >1 and <=4 => Math.Pow(3, levelTotal - 1) * 100,
                                       5 => 6500,
                                       6 => 14000,
                                       7 => 23000,
                                       8 => 34000,
                                       9 => 48000,
                                       10 => 64000,
                                       11 => 85000,
                                       12 => 100000,
                                       13 => 120000,
                                       14 => 140000,
                                       15 => 165000,
                                       16 => 195000,
                                       17 => 225000,
                                       18 => 265000,
                                       19 => 305000,
                                       20 => 355000,                                      
                                   };

            var stats = _context.CharacterAbility
                        .Where(cc => cc.CharacterId == characterId)
                        .Select(s => new {
                            s.Strength,
                            s.Dexterity,
                            s.Constitution,
                            s.Intelligence,
                            s.Wisdom,
                            s.Charisma
                        })
                        .FirstOrDefault();

            double modSTR = Math.Floor(((stats.Strength ?? 0) - 10) / 2.0);
            double modDEX = Math.Floor(((stats.Dexterity ?? 0 )- 10) / 2.0);
            double modCON = Math.Floor(((stats.Constitution ?? 0) - 10) / 2.0);
            double modINT = Math.Floor(((stats.Intelligence ?? 0) - 10) / 2.0);
            double modWIS = Math.Floor(((stats.Wisdom ?? 0) - 10) / 2.0);
            double modCHA = Math.Floor(((stats.Charisma ?? 0) - 10) / 2.0);

            var field_Race = String.IsNullOrEmpty(subspecie?.SubRaceName) ? $"{specie?.RaceName}" : $"{specie?.RaceName}({subspecie?.SubRaceName})";

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
                form.GetField("Race ")?.SetValue(field_Race);
                form.GetField("Alignment")?.SetValue(character.Alignment);
                form.GetField("ProfBonus")?.SetValue((1 + (int)Math.Ceiling(levelTotal / 4.0)).ToString()); 
                form.GetField("XP")?.SetValue(totalXP.ToString());
                
                form.GetField("STR")?.SetValue(stats.Strength.ToString());
                form.GetField("STRmod")?.SetValue($"{modSTR}");
                
                form.GetField("DEX")?.SetValue(stats.Dexterity.ToString());
                form.GetField("DEXmod ")?.SetValue($"{modDEX}");
                
                form.GetField("CON")?.SetValue(stats.Constitution.ToString());
                form.GetField("CONmod")?.SetValue($"{modCON}");
                
                form.GetField("INT")?.SetValue(stats.Intelligence.ToString());
                form.GetField("INTmod")?.SetValue($"{modINT}");
                
                form.GetField("WIS")?.SetValue(stats.Wisdom.ToString());
                form.GetField("WISmod")?.SetValue($"{modWIS}");
                
                form.GetField("CHA")?.SetValue(stats.Charisma.ToString());
                form.GetField("CHamod")?.SetValue($"{modCHA}");


                pdf.GetDocumentInfo().SetTitle(character?.CharacterName).SetSubject(character?.CharacterName);
            }

            var outputStream = new MemoryStream(tempStream.ToArray());
            outputStream.Position = 0;

            return File(outputStream, "application/pdf");
        }

    }
}

