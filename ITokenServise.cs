using System;
using AgendaxApi.Entities;

namespace AgendaxApi
{
	public interface ITokenServise
	{
        string CreateToke(User user);

    }
}

