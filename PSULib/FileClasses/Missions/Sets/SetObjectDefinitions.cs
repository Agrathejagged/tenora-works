using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Missions.Sets
{
    public class SetObjectDefinition
    {
        public readonly string name;
        public readonly int metadataLengthAotI;
        public readonly int metadataLengthPsp2;

        public SetObjectDefinition(string inName, int inLengthAotI, int inLengthPsp2)
        {
            name = inName;
            metadataLengthAotI = inLengthAotI;
            metadataLengthPsp2 = inLengthPsp2;
        }
    }

    public class SetObjectDefinitions
    {
        public static readonly Dictionary<int, SetObjectDefinition> definitions = new Dictionary<int, SetObjectDefinition>();

        static SetObjectDefinitions()
        {
            definitions[4] = new SetObjectDefinition("TObjUnbreak", 44, 44);
            definitions[5] = new SetObjectDefinition("TObjSwitchContact", 60, 64);
            definitions[6] = new SetObjectDefinition("TObjColliEffect", 64, 64);
            definitions[9] = new SetObjectDefinition("TObjColliEvent", 68, 72);
            definitions[10] = new SetObjectDefinition("TObjColliBlock", 52, 52);
            definitions[12] = new SetObjectDefinition("TObjBreak", 68, 68);
            definitions[14] = new SetObjectDefinition("TObjUnitTransporter", 92, 92);
            definitions[17] = new SetObjectDefinition("TObjFence", 60, 60);
            definitions[18] = new SetObjectDefinition("TObjNpc", 36, 36);
            definitions[20] = new SetObjectDefinition("TObjDoor", 28, 32);
            definitions[22] = new SetObjectDefinition("TObjSwitchTerminal", 44, 44);
            definitions[23] = new SetObjectDefinition("TEnemyGateway", 100, 100);
            definitions[24] = new SetObjectDefinition("TEnemyGateway", 100, 100);
            definitions[25] = new SetObjectDefinition("TEnemyGateway", 100, 100);
            definitions[26] = new SetObjectDefinition("TObjColliStartWithEvent", 144, 144);
            definitions[27] = new SetObjectDefinition("TObjColliGoalWithEvent", 108, 108);
            definitions[28] = new SetObjectDefinition("TObjColliPositionFlag", 4, 4);
            definitions[29] = new SetObjectDefinition("TObjColliPath", 12, 0);
            definitions[31] = new SetObjectDefinition("TObjKey", 24, 24);
            definitions[33] = new SetObjectDefinition("TObjColliCamera", 16, 16);
            definitions[35] = new SetObjectDefinition("TBossGate", 28, 28);
            definitions[37] = new SetObjectDefinition("TObjScenario", 8, 0);
            definitions[39] = new SetObjectDefinition("TObjDamage", 64, 80);
            definitions[40] = new SetObjectDefinition("TObjSavePoint", 16, 0);
            definitions[41] = new SetObjectDefinition("TObjTentacle", 56, 0);
            definitions[42] = new SetObjectDefinition("TMyRoomSetter", 32, 32);
            definitions[43] = new SetObjectDefinition("TObjFixedBattery", 112, 112);
            definitions[44] = new SetObjectDefinition("TObjBurstTrap", 120, 120);
            definitions[45] = new SetObjectDefinition("TObjColliShopCounter", 64, 64);
            definitions[46] = new SetObjectDefinition("TObjColliQuestCounter", 64, 64);
            definitions[47] = new SetObjectDefinition("TObjColliJobCounter", 56, 56);
            definitions[48] = new SetObjectDefinition("TObjBossTransporter", 60, 60);
            definitions[49] = new SetObjectDefinition("TObjCheckPoint", 60, 60);
            definitions[50] = new SetObjectDefinition("TObjCureMachine", 36, 36);
            definitions[51] = new SetObjectDefinition("TObjColliSearch", 60, 60);
            definitions[52] = new SetObjectDefinition("TObjSeedFlower", 76, 76);
            definitions[53] = new SetObjectDefinition("TObjNameBoard", 52, 52);
            definitions[54] = new SetObjectDefinition("TObjSeedCore", 92, 92);
            definitions[55] = new SetObjectDefinition("TObjPhotonPoint", 36, 36);
            definitions[56] = new SetObjectDefinition("TObjColliChair", 40, 40);
            definitions[57] = new SetObjectDefinition("TObjItem", 24, 0);
            definitions[58] = new SetObjectDefinition("TObjColliVehiclePos", 4, 0);
            definitions[59] = new SetObjectDefinition("TObjImageBoard", 60, 0);
            definitions[60] = new SetObjectDefinition("TObjServerTransporter", 36, 0);
            definitions[61] = new SetObjectDefinition("TObjLODModelPlayer & TObjLODModel", 12, 0);
            definitions[62] = new SetObjectDefinition("TObjCureMachinePP", 12, 12);
            definitions[63] = new SetObjectDefinition("TObjColliDressRoom", 8, 8);
            definitions[64] = new SetObjectDefinition("TObjRadarMapMarker", 44, 44);
            definitions[65] = new SetObjectDefinition("TMyRoomSetterIlminas", 24, 24);
            definitions[66] = new SetObjectDefinition("TSacredLotCounter", 56, 0);
            definitions[67] = new SetObjectDefinition("TRouletteCounter", 56, 0);
            definitions[68] = new SetObjectDefinition("TSlotMachineCounter", 60, 0);
            definitions[69] = new SetObjectDefinition("TMyRoomSetterCamera", 28, 28);
            definitions[70] = new SetObjectDefinition("TObjFixedBatteryIlminas", 124, 124);
            definitions[71] = new SetObjectDefinition("TObjBurstTrapIlminas", 136, 140);
            definitions[72] = new SetObjectDefinition("8TObjDrop", 0, 56);
            definitions[73] = new SetObjectDefinition("15TObjColliAttack", 0, 72);
            definitions[74] = new SetObjectDefinition("12TObjTrapPath", 0, 32);
            definitions[75] = new SetObjectDefinition("10TObjMoving", 0, 68);
            definitions[76] = new SetObjectDefinition("14TObjEnergyPole", 0, 24);
            definitions[77] = new SetObjectDefinition("21TObjBattleBaseBattery", 0, 24);
            definitions[78] = new SetObjectDefinition("22TObjBattleBaseMainUnit", 0, 68);
            definitions[79] = new SetObjectDefinition("19TObjBurstTrapSimple", 0, 44);
            definitions[80] = new SetObjectDefinition("16TObjPSP2Catapult", 0, 56);
            definitions[81] = new SetObjectDefinition("18TObjEnemyPheromone", 0, 40);
            definitions[82] = new SetObjectDefinition("22TObjFixedPlayerBattery", 0, 68);
            definitions[83] = new SetObjectDefinition("14TObjBattleWall", 0, 92);
            definitions[84] = new SetObjectDefinition("9TObjSwing", 0, 88);
            definitions[85] = new SetObjectDefinition("11TObjRolling", 0, 92);
            definitions[86] = new SetObjectDefinition("12TObjRoulette", 0, 60);
            definitions[87] = new SetObjectDefinition("13TObjAnimation", 0, 56);
            definitions[88] = new SetObjectDefinition("15TObjSwitchParty", 0, 108);
            definitions[89] = new SetObjectDefinition("8TObjChat", 0, 88);
            definitions[90] = new SetObjectDefinition("9TObjGrave & 10TObjWindow", 0, 8);
            definitions[91] = new SetObjectDefinition("9TObjPress", 0, 124);
            definitions[92] = new SetObjectDefinition("10TObjNeedle", 0, 108);
            definitions[93] = new SetObjectDefinition("21TObjCureMachineAccess", 0, 36);
            definitions[94] = new SetObjectDefinition("16TObjEnergyPoleEx", 0, 72);
            definitions[95] = new SetObjectDefinition("13TObjCityBoard", 0, 20);
            definitions[96] = new SetObjectDefinition("14TObjMesetaDrop", 0, 28);
            definitions[97] = new SetObjectDefinition("17TObjColliCityFlag", 0, 52);
        }

    }
}
