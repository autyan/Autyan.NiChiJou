﻿using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJou.Model.Blog
{
    public class PostComment : LongKeyBaseEntity
    {
        public virtual string Content { get; set; }

        public virtual long? BlogUserId { get; set; }

        public virtual BlogUser BlogUser { get; set; }

        public virtual long? PostId { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public virtual long? ToComment { get; set; }
    }

    public class PostCommentQuery : LongKeyBaseEntityQuery
    {
        public string Content { get; set; }

        public long? BlogUserId { get; set; }


        public long? PostId { get; set; }


        public long? ToComment { get; set; }
    }
}
