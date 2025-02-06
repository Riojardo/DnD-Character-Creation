# DnD-Character-Creation
online d&amp;d character creator 

<!--
	#_1_ Core Tables

	Users (Stores player information)

user_id (Primary Key, Unique)
username
email
password_hash
created_at
last_login

	Characters (Stores individual character data)

character_id (Primary Key, Unique)
user_id (Foreign Key ? Users)
name
race_id (Foreign Key ? Races)
class_id (Foreign Key ? Classes)
background_id (Foreign Key ? Backgrounds)
alignment
level
experience_points
created_at
updated_at

	##_2_ D&D 5e Specific Tables

	Races

race_id (Primary Key, Unique)
name
description
size
speed
ability_bonuses (e.g., JSON { "STR": 2, "DEX": 1 })
features (Text or Foreign Key to Racial_Features)

	Classes

class_id (Primary Key, Unique)
name
hit_die
primary_ability
saving_throws
proficiencies
subclass_id (Foreign Key ? Subclasses)

	Subclasses

subclass_id (Primary Key, Unique)
class_id (Foreign Key ? Classes)
name
description
features (Text or JSON)

	Backgrounds

background_id (Primary Key, Unique)
name
description
skills
equipment
feature
languages

	Abilities (Stats)

character_id (Foreign Key ? Characters)
strength
dexterity
constitution
intelligence
wisdom
charisma

	Skills

skill_id (Primary Key, Unique)
name
ability_modifier (Foreign Key ? Abilities)
description

	Character_Skills (Junction Table for skills proficiency)

character_id (Foreign Key ? Characters)
skill_id (Foreign Key ? Skills)
proficiency_bonus

	#_3_Equipment & Items

	Equipment

equipment_id (Primary Key, Unique)
name
type (Weapon, Armor, Tool, etc.)
cost
weight
properties

	Character_Equipment (Junction Table for characters' equipment)

character_id (Foreign Key ? Characters)
equipment_id (Foreign Key ? Equipment)
quantity

	#_4_Spells & Features

	Spells

spell_id (Primary Key, Unique)
name
level
casting_time
range
components
duration
description

	Character_Spells (Junction Table for character spellcasting)

character_id (Foreign Key ? Characters)
spell_id (Foreign Key ? Spells)
prepared (Boolean)

	#_5_Feats & Traits

	Feats

feat_id (Primary Key, Unique)
name
description
requirements

	Character_Feats (Junction Table for character feats)

character_id (Foreign Key ? Characters)
feat_id (Foreign Key ? Feats)

	Racial_Features (Racial abilities like Darkvision)

feature_id (Primary Key, Unique)
race_id (Foreign Key ? Races)
name
description
		
		#_6_Campaign & Roleplay

	Campaigns
campaign_id (Primary Key, Unique)
name
dm_id (Foreign Key ? Users)
description

	Character_Campaigns (Junction Table for campaign membership)
character_id (Foreign Key ? Characters)
campaign_id (Foreign Key ? Campaigns)
	
		#_7_Optional Tables for Advanced Features

	Inventory (Detailed version of Equipment)
inventory_id (Primary Key)
character_id (Foreign Key ? Characters)
item_name
quantity
description

	Quest_Log
quest_id (Primary Key, Unique)
character_id (Foreign Key ? Characters)
quest_name
status (Active, Completed, Failed)
reward



		What This Schema Achieves
		? Normalized Data Structure – Eliminates redundancy.
		? Scalability – Easily expandable for new features like homebrew content.
		? Relational Integrity – Uses Foreign Keys to ensure data consistency.
		? Flexibility – Allows players to create, edit, and manage characters dynamically.		
-->
