﻿namespace OpenSage.Logic.Object.Helpers
{
    internal sealed class ObjectWeaponStatusHelper : ObjectHelperModule
    {
        // TODO
        protected override UpdateOrder UpdateOrder => UpdateOrder.Order3;

        internal override void Load(StatePersister reader)
        {
            reader.PersistVersion(1);

            reader.BeginObject("Base");
            base.Load(reader);
            reader.EndObject();
        }
    }
}
