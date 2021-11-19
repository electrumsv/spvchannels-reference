// Copyright(c) 2020 Bitcoin Association.
// Distributed under the Open BSV software license, see the accompanying file LICENSE

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using SPVChannels.API.Rest.ViewModel;
using SPVChannels.Domain.Repositories;
using SPVChannels.Infrastructure.Auth;
using SPVChannels.Infrastructure.Utilities;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SPVChannels.API.Rest.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/account")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    readonly ILogger<ChannelController> logger;
    readonly AppConfiguration configuration;
    readonly IAccountRepository account;
    public AccountController(
      ILogger<ChannelController> logger,
      IOptions<AppConfiguration> options,
      IAccountRepository account)
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.account = account;
      if (options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }
      else
      {
        if(options.Value == null)
          throw new ArgumentNullException(nameof(AppConfiguration));

        configuration = options.Value;
      }
    }

    #region Accounts

    private static long InitializeCreateAccount(IAccountRepository accountRep, string accountname, string userName, string userPassword)
    {
      var accountId = accountRep.CreateAccount(accountname.Replace('_', ' '), "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{userPassword}")));
      Console.WriteLine($"{accountname} was created with account-id:{accountId}");

      return accountId;
    }

    private bool CheckMasterAccountCreationToken(string authHeader)
    {
      var masterAccountCreationToken = configuration.MasterAccountCreationToken;
      string bearerToken = authHeader.Substring("Bearer ".Length).Trim();
      if (masterAccountCreationToken != bearerToken)
      {
        return false;
      }
      return true;
    }

    // POST: /api/v1/account
    /// <summary>
    /// Create a new account (Restricted Access - requires MasterAccountCreationToken as Bearer Token).
    /// </summary>
    /// <returns>Account ID, Account Name</returns>
    [HttpPost("")]
    public ActionResult<ChannelViewModelAccountGet> CreateAccount([FromBody]ChannelViewModelAccountCreate data)
    {
      logger.LogInformation($"Create a new account.");
      string authHeader = Request.Headers["Authorization"];
      bool authSuccess = this.CheckMasterAccountCreationToken(authHeader);
      if (!authSuccess) {
        return Unauthorized("Invalid token.");
      }
      var accountId = InitializeCreateAccount(account, data.AccountName, data.AccountUsername, data.AccountPassword);
      logger.LogInformation($"Returning account id: {accountId} account name: {data.AccountName}.");
      return Ok(new ChannelViewModelAccountGet(accountId.ToString(), data.AccountName));
    }

    #endregion
  }
}
