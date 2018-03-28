using System;
using System.Collections;
using System.Collections.Generic;
using com.ihaiu;
using Games;
namespace Games
{
	public partial class ConfigManager
	{

		public MsgConfigReader	msg	= new MsgConfigReader();
		public SkillConfigReader	skill	= new SkillConfigReader();
		public SkillEffectConfigReader	skillEffect	= new SkillEffectConfigReader();
		public ProtoRegisterReader	protoRegisterReader	= new ProtoRegisterReader();
		public PropConfigReader	prop	= new PropConfigReader();
		public ProtoOpcodeReader	protoOpcodeReader	= new ProtoOpcodeReader();
		public SkillLevelConfigReader	skillLevel	= new SkillLevelConfigReader();
		public UnitConfigReader	unit	= new UnitConfigReader();
		public UnitLevelConfigReader	unitLevel	= new UnitLevelConfigReader();
		public SkillValueConfigReader	skillValue	= new SkillValueConfigReader();
		public SkillLinkConfigReader	skillLink	= new SkillLinkConfigReader();
		public SoundConfigReader	sound	= new SoundConfigReader();
		public StageConfigReader	stage	= new StageConfigReader();
		public AIScoreDistanceConfigReader	aIScoreDistance	= new AIScoreDistanceConfigReader();
		public AIScoreUnitTypeCofnigRenader	aIScoreUnitTypeCofnigRenader	= new AIScoreUnitTypeCofnigRenader();
		public AIScoreWeightConfigReader	aIScoreWeight	= new AIScoreWeightConfigReader();
		public AIHeroConfigReader	aIHero	= new AIHeroConfigReader();
		public AIScoreAttackHatredConfigReader	aIScoreAttackHatred	= new AIScoreAttackHatredConfigReader();
		public AIScoreConfigReader	aIScore	= new AIScoreConfigReader();
		public AISoliderConfigReader	aISolider	= new AISoliderConfigReader();
		public GlobalConfigConfigReader	globalConfig	= new GlobalConfigConfigReader();
		public LoadConfigReader	load	= new LoadConfigReader();
		public MenuConfigReader	menu	= new MenuConfigReader();
		public AttributePackConfigReader	attributePack	= new AttributePackConfigReader();
		public AvatarConfigReader	avatar	= new AvatarConfigReader();
		public DungeonConfigReader	dungeon	= new DungeonConfigReader();


		private List<IConfigReader> _l;
		public List<IConfigReader> readerList
		{
			get
			{
				if(_l == null)
				{
					_l = new List<IConfigReader>();
					//_l.Add(msg);
					_l.Add(skill);
					//_l.Add(skillEffect);
					//_l.Add(protoRegisterReader);
					_l.Add(prop);
					//_l.Add(protoOpcodeReader);
					_l.Add(skillLevel);
					_l.Add(unit);
					_l.Add(unitLevel);
					_l.Add(skillValue);
					_l.Add(skillLink);
					_l.Add(sound);
					//_l.Add(stage);
					_l.Add(aIScoreDistance);
					_l.Add(aIScoreUnitTypeCofnigRenader);
					_l.Add(aIScoreWeight);
					_l.Add(aIHero);
					_l.Add(aIScoreAttackHatred);
					//_l.Add(aIScore);
					_l.Add(aISolider);
					_l.Add(globalConfig);
					//_l.Add(load);
					//_l.Add(menu);
					_l.Add(attributePack);
					_l.Add(avatar);
					_l.Add(dungeon);

				}
				return _l;
			}
		}
	}
}
