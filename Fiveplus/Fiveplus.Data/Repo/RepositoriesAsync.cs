using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.Models;
using Fiveplus.Data.Uow;

namespace Fiveplus.Data.Repo
{
    /*
    public interface IUserInboxMessageGigRepositoryAsync : IEntityRepositoryAsync<UserInboxMessage> { }
    public interface IUserCollectedGigRepositoryAsync : IEntityRepositoryAsync<UserCollectedGig> { }
    public interface IUserDetailRepositoryAsync : IEntityRepositoryAsync<UserDetail> { }

    public interface ICategoryRepositoryAsync : IEntityRepositoryAsync<Category> { }
    public interface IAddonRepositoryAsync : IEntityRepositoryAsync<Addon> { }
    public interface IMediaRepositoryAsync : IEntityRepositoryAsync<Media>{}
    public interface IGigRepositoryAsync : IEntityRepositoryAsync<Gig> { }*/

    public class MediaRepositoryAsync : BaseRepository<Media>, IMediaRepositoryAsync
    {
       public MediaRepositoryAsync(ExplorerUow explorerUow): base(explorerUow){}
    }

    public class AddonRepositoryAsync : BaseRepository<Addon>, IAddonRepositoryAsync
    {
        public AddonRepositoryAsync(ExplorerUow explorerUow) : base(explorerUow) { }
    }

    public class CategoryRepositoryAsync : BaseRepository<Category>, ICategoryRepositoryAsync
    {
        public CategoryRepositoryAsync(ExplorerUow explorerUow) : base(explorerUow) { }
    }

    public class UserDetailRepositoryAsync : BaseRepository<UserDetail>, IUserDetailRepositoryAsync
    {
        public UserDetailRepositoryAsync(ExplorerUow explorerUow) : base(explorerUow) { }
    }

    public class UserCollectedGigRepositoryAsync : BaseRepository<UserCollectedGig>, IUserCollectedGigRepositoryAsync
    {
        public UserCollectedGigRepositoryAsync(ExplorerUow explorerUow) : base(explorerUow) { }
    }

    public class UserInboxMessageGigRepositoryAsync : BaseRepository<UserInboxMessage>, IUserInboxMessageRepositoryAsync
    {
        public UserInboxMessageGigRepositoryAsync(ExplorerUow explorerUow) : base(explorerUow) { }
    }

}
