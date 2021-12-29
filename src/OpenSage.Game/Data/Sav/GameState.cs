﻿using System;

namespace OpenSage.Data.Sav
{
    public sealed class GameState
    {
        public SaveGameType GameType;
        public string MapPath;
        public DateTime Timestamp;
        public string DisplayName;
        public string MapFileName;
        public string Side;
        public uint MissionIndex;

        internal void Load(SaveFileReader reader)
        {
            reader.ReadVersion(2);

            reader.ReadEnum(ref GameType);
            reader.ReadAsciiString(ref MapPath);
            reader.ReadDateTime(ref Timestamp);
            reader.ReadUnicodeString(ref DisplayName);
            reader.ReadAsciiString(ref MapFileName);
            reader.ReadAsciiString(ref Side);
            reader.ReadUInt32(ref MissionIndex);
        }
    }
}
