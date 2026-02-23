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
                                            classInfos.Add($"{classEntity.Name}({cc.Level})");
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

            var firstClass = _context.Class.SingleOrDefault(c => c.ClassId == characterClasses[0].ClassId);
            var secondClass = characterClasses.Count >=2 ? _context.Class.SingleOrDefault(c => c.ClassId == characterClasses[1].ClassId) : null;
            var thirdClass = characterClasses.Count == 3 ? _context.Class.SingleOrDefault(c => c.ClassId == characterClasses[2].ClassId) : null;

            bool saveSTR = (firstClass.PrimaryAbility == "Strength" || firstClass.SecondaryAbility == "Strength");
            bool saveDEX = (firstClass.PrimaryAbility == "Dexterity" || firstClass.SecondaryAbility == "Dexterity");
            bool saveCON = (firstClass.PrimaryAbility == "Constitution" || firstClass.SecondaryAbility == "Constitution");
            bool saveINT = (firstClass.PrimaryAbility == "Intelligence" || firstClass.SecondaryAbility == "Intelligence");
            bool saveWIS = (firstClass.PrimaryAbility == "Wisdom" || firstClass.SecondaryAbility == "Wisdom");
            bool saveCHA = (firstClass.PrimaryAbility == "Charisma" || firstClass.SecondaryAbility == "Charisma");


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

            var skills = await _context.CharacterSkill.FirstOrDefaultAsync(c => c.CharacterId == characterId);

            int proficiencyBonus = ((1 + (int)Math.Ceiling(levelTotal / 4.0)));

            double modSTR = Math.Floor(((stats.Strength ?? 0) - 10) / 2.0);
            double modDEX = Math.Floor(((stats.Dexterity ?? 0 )- 10) / 2.0);
            double modCON = Math.Floor(((stats.Constitution ?? 0) - 10) / 2.0);
            double modINT = Math.Floor(((stats.Intelligence ?? 0) - 10) / 2.0);
            double modWIS = Math.Floor(((stats.Wisdom ?? 0) - 10) / 2.0);
            double modCHA = Math.Floor(((stats.Charisma ?? 0) - 10) / 2.0);

            var hitPoint = (decimal)(firstClass?.HitDice ?? 0) + ((decimal)levelTotal * (decimal)modCON) +
                           (decimal)((((characterClasses.ElementAtOrDefault(0)?.Level - 1) ?? 0) * (((firstClass?.HitDice ?? 0) / 2.0) + 1))) +
                           (decimal)(((characterClasses.ElementAtOrDefault(1)?.Level ?? 0) * (((secondClass?.HitDice ?? 0) / 2.0) + 1))) +
                           (decimal)(((characterClasses.ElementAtOrDefault(2)?.Level ?? 0) * (((thirdClass?.HitDice ?? 0) / 2.0) + 1)));

            var field_Race = String.IsNullOrEmpty(subspecie?.SubRaceName) ? $"{specie?.RaceName}" : $"{specie?.RaceName}({subspecie?.SubRaceName})";

            if (character == null)
                return NotFound();


           ///pdfEditor

            var templatePath = "Content/pdf/5E_CharacterSheet_Fillable.pdf";

            using var tempStream = new MemoryStream();
            using (var reader = new PdfReader(templatePath))
            using (var writer = new PdfWriter(tempStream))
            using (var pdf = new PdfDocument(reader, writer))
            {
                var form = PdfAcroForm.GetAcroForm(pdf, true);
                form.GetField("PlayerName")?.SetValue(character.CharacterName);
                form.GetField("CharacterName")?.SetValue(character.CharacterName);
                form.GetField("ClassLevel")?.SetValue(string.Join(" /", classInfos));
                form.GetField("Background")?.SetValue(background?.Name);
                form.GetField("Race ")?.SetValue(field_Race);
                form.GetField("Alignment")?.SetValue(character.Alignment);
                form.GetField("ProfBonus")?.SetValue(proficiencyBonus.ToString()); 
                form.GetField("XP")?.SetValue(totalXP.ToString());
                form.GetField("HPMax")?.SetValue(hitPoint.ToString()); 
                form.GetField("HPCurrent")?.SetValue(hitPoint.ToString());

                //Ability

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

                //SaveDice

                form.GetField("ST Strength")?.SetValue(saveSTR ? (modSTR + proficiencyBonus).ToString() : modSTR.ToString());
                form.GetField("Check Box 11")?.SetValue(saveSTR ? "Yes" : "Off");

                form.GetField("ST Dexterity")?.SetValue(saveDEX ? (modDEX + proficiencyBonus).ToString() : modDEX.ToString());
                form.GetField("Check Box 18")?.SetValue(saveDEX ? "Yes" : "Off");

                form.GetField("ST Constitution")?.SetValue(saveCON ? (modCON + proficiencyBonus).ToString() : modCON.ToString());
                form.GetField("Check Box 19")?.SetValue(saveCON ? "Yes" : "Off");

                form.GetField("ST Intelligence")?.SetValue(saveINT ? (modINT + proficiencyBonus).ToString() : modINT.ToString());
                form.GetField("Check Box 20")?.SetValue(saveINT ? "Yes" : "Off");

                form.GetField("ST Wisdom")?.SetValue(saveWIS ? (modWIS + proficiencyBonus).ToString() : modWIS.ToString());
                form.GetField("Check Box 21")?.SetValue(saveWIS ? "Yes" : "Off");

                form.GetField("ST Charisma")?.SetValue(saveCHA ? (modCHA + proficiencyBonus).ToString() : modCHA.ToString());
                form.GetField("Check Box 22")?.SetValue(saveCHA ? "Yes" : "Off");

                //Skills

                form.GetField("Acrobatics")?.SetValue(skills.Acrobatics ? (modDEX + proficiencyBonus).ToString() : modDEX.ToString());
                form.GetField("Check Box 23")?.SetValue(skills.Acrobatics ? "Yes" : "Off");

                form.GetField("Animal")?.SetValue(skills.AnimalHandling ? (modWIS + proficiencyBonus).ToString() : modWIS.ToString());
                form.GetField("Check Box 24")?.SetValue(skills.AnimalHandling ? "Yes" : "Off");

                form.GetField("Arcana")?.SetValue(skills.Arcana ? (modINT + proficiencyBonus).ToString() : modINT.ToString());
                form.GetField("Check Box 25")?.SetValue(skills.Arcana ? "Yes" : "Off");

                form.GetField("Athletics")?.SetValue(skills.Athletics ? (modSTR + proficiencyBonus).ToString() : modSTR.ToString());
                form.GetField("Check Box 26")?.SetValue(skills.Athletics ? "Yes" : "Off");

                form.GetField("Deception ")?.SetValue(skills.Deception ? (modCHA + proficiencyBonus).ToString() : modCHA.ToString());
                form.GetField("Check Box 27")?.SetValue(skills.Deception ? "Yes" : "Off");

                form.GetField("History ")?.SetValue(skills.History ? (modINT + proficiencyBonus).ToString() : modINT.ToString());
                form.GetField("Check Box 28")?.SetValue(skills.History ? "Yes" : "Off");

                form.GetField("Insight")?.SetValue(skills.Insight ? (modWIS + proficiencyBonus).ToString() : modWIS.ToString());
                form.GetField("Check Box 29")?.SetValue(skills.Insight ? "Yes" : "Off");

                form.GetField("Intimidation")?.SetValue(skills.Intimidation ? (modCHA + proficiencyBonus).ToString() : modCHA.ToString());
                form.GetField("Check Box 30")?.SetValue(skills.Intimidation ? "Yes" : "Off");

                form.GetField("Investigation ")?.SetValue(skills.Investigation ? (modINT + proficiencyBonus).ToString() : modINT.ToString());
                form.GetField("Check Box 31")?.SetValue(skills.Investigation ? "Yes" : "Off");

                form.GetField("Medicine")?.SetValue(skills.Medicine ? (modWIS + proficiencyBonus).ToString() : modWIS.ToString());
                form.GetField("Check Box 32")?.SetValue(skills.Medicine ? "Yes" : "Off");

                form.GetField("Nature")?.SetValue(skills.Nature ? (modINT + proficiencyBonus).ToString() : modINT.ToString());
                form.GetField("Check Box 33")?.SetValue(skills.Nature ? "Yes" : "Off");

                form.GetField("Perception ")?.SetValue(skills.Perception ? (modWIS + proficiencyBonus).ToString() : modWIS.ToString());
                form.GetField("Check Box 34")?.SetValue(skills.Perception ? "Yes" : "Off");

                form.GetField("Performance")?.SetValue(skills.Performance ? (modCHA + proficiencyBonus).ToString() : modCHA.ToString());
                form.GetField("Check Box 35")?.SetValue(skills.Performance ? "Yes" : "Off");

                form.GetField("Persuasion")?.SetValue(skills.Persuasion ? (modCHA + proficiencyBonus).ToString() : modCHA.ToString());
                form.GetField("Check Box 36")?.SetValue(skills.Persuasion ? "Yes" : "Off");

                form.GetField("Religion")?.SetValue(skills.Religion ? (modINT + proficiencyBonus).ToString() : modINT.ToString());
                form.GetField("Check Box 37")?.SetValue(skills.Religion ? "Yes" : "Off");

                form.GetField("SleightofHand")?.SetValue(skills.SleightOfHand ? (modDEX + proficiencyBonus).ToString() : modDEX.ToString());
                form.GetField("Check Box 38")?.SetValue(skills.SleightOfHand ? "Yes" : "Off");

                form.GetField("Stealth ")?.SetValue(skills.Stealth ? (modDEX + proficiencyBonus).ToString() : modDEX.ToString());
                form.GetField("Check Box 39")?.SetValue(skills.Stealth ? "Yes" : "Off");

                form.GetField("Survival")?.SetValue(skills.Survival ? (modWIS + proficiencyBonus).ToString() : modWIS.ToString());
                form.GetField("Check Box 40")?.SetValue(skills.Survival ? "Yes" : "Off");

                pdf.GetDocumentInfo().SetTitle(character?.CharacterName).SetSubject(character?.CharacterName);
            }

            var outputStream = new MemoryStream(tempStream.ToArray());
            outputStream.Position = 0;

            return File(outputStream, "application/pdf");
        }

    }
}

