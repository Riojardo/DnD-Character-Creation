using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DnD_CharacterCreation.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Background",
                columns: table => new
                {
                    BackgroundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimarySkill = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondarySkill = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Background", x => x.BackgroundId);
                });

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HitDice = table.Column<int>(type: "int", nullable: true),
                    PrimaryAbility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryAbility = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.ClassId);
                });

            migrationBuilder.CreateTable(
                name: "Feat",
                columns: table => new
                {
                    FeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feat", x => x.FeatId);
                });

            migrationBuilder.CreateTable(
                name: "Race",
                columns: table => new
                {
                    RaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Race", x => x.RaceId);
                });

            migrationBuilder.CreateTable(
                name: "Spell",
                columns: table => new
                {
                    SpellId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpellLevel = table.Column<int>(type: "int", nullable: true),
                    SpellSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spell", x => x.SpellId);
                });

            migrationBuilder.CreateTable(
                name: "SubRace",
                columns: table => new
                {
                    SubRaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubRaceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubRace", x => x.SubRaceId);
                    table.ForeignKey(
                        name: "FK_SubRace_Race_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Race",
                        principalColumn: "RaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alignment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackgroundId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackgroundId1 = table.Column<int>(type: "int", nullable: true),
                    RaceId = table.Column<int>(type: "int", nullable: true),
                    SubRaceId = table.Column<int>(type: "int", nullable: true),
                    LevelTotal = table.Column<int>(type: "int", nullable: true),
                    HitPoints = table.Column<int>(type: "int", nullable: true),
                    ArmorClass = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_Characters_Background_BackgroundId1",
                        column: x => x.BackgroundId1,
                        principalTable: "Background",
                        principalColumn: "BackgroundId");
                    table.ForeignKey(
                        name: "FK_Characters_Race_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Race",
                        principalColumn: "RaceId");
                    table.ForeignKey(
                        name: "FK_Characters_SubRace_SubRaceId",
                        column: x => x.SubRaceId,
                        principalTable: "SubRace",
                        principalColumn: "SubRaceId");
                });

            migrationBuilder.CreateTable(
                name: "CharacterAbility",
                columns: table => new
                {
                    CharacterAbilityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: true),
                    Dexterity = table.Column<int>(type: "int", nullable: true),
                    Constitution = table.Column<int>(type: "int", nullable: true),
                    Intelligence = table.Column<int>(type: "int", nullable: true),
                    Wisdom = table.Column<int>(type: "int", nullable: true),
                    Charisma = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAbility", x => x.CharacterAbilityId);
                    table.ForeignKey(
                        name: "FK_CharacterAbility_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterClass",
                columns: table => new
                {
                    CharacterClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClass", x => x.CharacterClassId);
                    table.ForeignKey(
                        name: "FK_CharacterClass_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterClass_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterFeat",
                columns: table => new
                {
                    CharacterFeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    FeatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterFeat", x => x.CharacterFeatId);
                    table.ForeignKey(
                        name: "FK_CharacterFeat_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterFeat_Feat_FeatId",
                        column: x => x.FeatId,
                        principalTable: "Feat",
                        principalColumn: "FeatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSkill",
                columns: table => new
                {
                    CharacterSkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    Acrobatics = table.Column<bool>(type: "bit", nullable: false),
                    AnimalHandling = table.Column<bool>(type: "bit", nullable: false),
                    Arcana = table.Column<bool>(type: "bit", nullable: false),
                    Athletics = table.Column<bool>(type: "bit", nullable: false),
                    Deception = table.Column<bool>(type: "bit", nullable: false),
                    History = table.Column<bool>(type: "bit", nullable: false),
                    Insight = table.Column<bool>(type: "bit", nullable: false),
                    Intimidation = table.Column<bool>(type: "bit", nullable: false),
                    Investigation = table.Column<bool>(type: "bit", nullable: false),
                    Medicine = table.Column<bool>(type: "bit", nullable: false),
                    Nature = table.Column<bool>(type: "bit", nullable: false),
                    Perception = table.Column<bool>(type: "bit", nullable: false),
                    Performance = table.Column<bool>(type: "bit", nullable: false),
                    Persuasion = table.Column<bool>(type: "bit", nullable: false),
                    Religion = table.Column<bool>(type: "bit", nullable: false),
                    SleightOfHand = table.Column<bool>(type: "bit", nullable: false),
                    Stealth = table.Column<bool>(type: "bit", nullable: false),
                    Survival = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSkill", x => x.CharacterSkillId);
                    table.ForeignKey(
                        name: "FK_CharacterSkill_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSpell",
                columns: table => new
                {
                    CharacterSpellId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    SpellId = table.Column<int>(type: "int", nullable: false),
                    IsPrepared = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSpell", x => x.CharacterSpellId);
                    table.ForeignKey(
                        name: "FK_CharacterSpell_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSpell_Spell_SpellId",
                        column: x => x.SpellId,
                        principalTable: "Spell",
                        principalColumn: "SpellId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAbility_CharacterId",
                table: "CharacterAbility",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClass_CharacterId",
                table: "CharacterClass",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClass_ClassId",
                table: "CharacterClass",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterFeat_CharacterId",
                table: "CharacterFeat",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterFeat_FeatId",
                table: "CharacterFeat",
                column: "FeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_BackgroundId1",
                table: "Characters",
                column: "BackgroundId1");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_RaceId",
                table: "Characters",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SubRaceId",
                table: "Characters",
                column: "SubRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSkill_CharacterId",
                table: "CharacterSkill",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSpell_CharacterId",
                table: "CharacterSpell",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSpell_SpellId",
                table: "CharacterSpell",
                column: "SpellId");

            migrationBuilder.CreateIndex(
                name: "IX_SubRace_RaceId",
                table: "SubRace",
                column: "RaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterAbility");

            migrationBuilder.DropTable(
                name: "CharacterClass");

            migrationBuilder.DropTable(
                name: "CharacterFeat");

            migrationBuilder.DropTable(
                name: "CharacterSkill");

            migrationBuilder.DropTable(
                name: "CharacterSpell");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Feat");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Spell");

            migrationBuilder.DropTable(
                name: "Background");

            migrationBuilder.DropTable(
                name: "SubRace");

            migrationBuilder.DropTable(
                name: "Race");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
