using System;

using ANT.ORM.Models;

namespace ANT.ORM
{
    public abstract class DbEntity : IDbEntity
    {
        private DbEntityMetadata? metadata;
        public DbEntityMetadata Metadata
        {
            get
            {
                if (metadata == null)
                    if ((metadata = AntProvider.GetEntityMetadata(this.GetType())) == null)
                        throw new InvalidOperationException("Entity class is not registered in ANTProvider");
                return metadata;
            }
        }
    }
}