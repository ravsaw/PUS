﻿namespace PUS.Models
{
    public enum Status : int
    {
        Unknow = 999,
        Success = 0,

        //Login section
        WrongEmailOrPassword = 1,
        Locked = 2,

        //Register section
        EmailNeedConfirm = 100,
        EmailInUser = 101,

        //Chats
        UserShouldntTalkWithHimself = 200,

        //Transactions
        TransactionsNotFound = 300,
        TransactionNotFound = 301,
    }

    public class AccountRequestStatus
    {
        public Status Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
