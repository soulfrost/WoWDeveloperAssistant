﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WoWDeveloperAssistant.Packets;

namespace WoWDeveloperAssistant.Waypoints_Creator
{
    public struct WaypointScript : ICloneable
    {
        public enum ScriptType : byte
        {
            Talk           = 0,
            Emote          = 1,
            SetField       = 2,
            SetFlag        = 4,
            RemoveFlag     = 5,
            RemoveAura     = 14,
            CastSpell      = 15,
            SetOrientation = 30,
            SetAnimKit     = 100,
            Unknown        = 99
        };

        public uint id;
        public uint delay;
        public ScriptType type;
        public uint dataLong;
        public uint dataLongSecond;
        public uint dataInt;
        public float x;
        public float y;
        public float z;
        public float o;
        public uint guid;
        public TimeSpan scriptTime;

        WaypointScript(uint id, uint delay, ScriptType type, uint dataLong, uint dataLongSecond, uint dataInt, float x, float y, float z, float o, uint guid, TimeSpan time)
        { this.id = id; this.delay = delay; this.type = type; this.dataLong = dataLong; this.dataLongSecond = dataLongSecond; this.dataInt = dataInt; this.x = x; this.y = y; this.z = z; this.o = o; this.guid = guid; this.scriptTime = time; }

        public static List<WaypointScript> GetScriptsFromUpdatePacket(UpdateObjectPacket updatePacket)
        {
            List<WaypointScript> waypointScripts = new List<WaypointScript>();

            if (updatePacket.emoteStateId != null)
                waypointScripts.Add(new WaypointScript(0, 0, ScriptType.Emote, (uint)updatePacket.emoteStateId, 1, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0, updatePacket.packetSendTime));

            if (updatePacket.sheatheState != null)
                waypointScripts.Add(new WaypointScript(0, 0, ScriptType.SetField, 117, (uint)updatePacket.sheatheState, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0, updatePacket.packetSendTime));

            return waypointScripts;
        }

        public static WaypointScript GetScriptsFromSpellPacket(SpellStartPacket spellPacket)
        {
            return new WaypointScript(0, 0, ScriptType.CastSpell, spellPacket.spellId, 1, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0, spellPacket.spellCastStartTime);
        }

        public static WaypointScript GetScriptsFromMovementPacket(MonsterMovePacket movePacket)
        {
            return new WaypointScript(0, 0, ScriptType.SetOrientation, 0, 0, 0, 0.0f, 0.0f, 0.0f, movePacket.creatureOrientation, 0, movePacket.packetSendTime);
        }

        public static WaypointScript GetScriptsFromAuraUpdatePacket(AuraUpdatePacket auraPacket, Creature creature)
        {
            return new WaypointScript(0, 0, ScriptType.RemoveAura, creature.GetSpellIdForAuraSlot(auraPacket), 1, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0, auraPacket.packetSendTime);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void SetId(uint id)
        {
            this.id = id;
        }

        public void SetGuid(uint guid)
        {
            this.guid = guid;
        }

        public void SetDelay(uint delay)
        {
            this.delay = delay;
        }
    }
}
