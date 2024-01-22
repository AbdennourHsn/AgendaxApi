using System;
using AgendaxApi.Entities;

namespace AgendaxApi.Interfaces
{
	public interface ITokenServise
	{
        string CreateToke(User user);

    }
}

