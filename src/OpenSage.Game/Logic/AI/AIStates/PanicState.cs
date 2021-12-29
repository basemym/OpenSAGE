﻿using OpenSage.Data.Sav;

namespace OpenSage.Logic.AI.AIStates
{
    internal sealed class PanicState : FollowWaypointsState
    {
        private uint _unknownInt1;
        private uint _unknownInt2;

        public PanicState()
            : base(false)
        {

        }

        internal override void Load(SaveFileReader reader)
        {
            reader.ReadVersion(1);

            base.Load(reader);

            reader.ReadUInt32(ref _unknownInt1);
            reader.ReadUInt32(ref _unknownInt2);
        }
    }
}
