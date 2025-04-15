export const errorCodeDefine = {
    //預設
    Default: 0,

    //成功
    Success: 1,

    //被他人踢出
    KickOut: 2,

    //被Ban掉
    Baned: 3,

    //權限已被修改
    PermissionModified: 4,

    //無效輸入
    InvalidFormatOrEntry: 5,

    //伺服器錯誤
    ServerError: 6,

    //無權限
    NoPermission: 7,

    //使用者未登入
    UserNotLogged: 8,

    //登入失敗
    LoginFailed: 9,

    //密碼輸入錯誤
    PasswordEnterIncorrectly: 10,

    //創建失敗
    CreateFailed: 11,

    //修改失敗
    ModifiedFailed: 12,

    //刪除失敗
    DeleteFailed: 13
};

//設定errorCode對應資料
export function errorCodeToMessage(errorCode) {
    let message;

    switch (errorCode) {
        case 1:
            message = "請求成功";
            return message;
        case 2:
            message = "您的帳號已被其他使用者踢出";
            return message;
        case 3:
            message = "您的帳號已被禁用";
            return message;
        case 4:
            message = "您的權限已被更動，請重新登入";
            return message;
        case 5:
            message = "請求格式錯誤或無效數據";
            return message;
        case 6:
            message = "伺服器錯誤";
            return message;
        case 7:
            message = "沒有此權限";
            return message;
        case 8:
            message = "使用者未登入";
            return message;
        case 9:
            message = "登入失敗";
            return message;
        case 10:
            message = "密碼輸入錯誤";
            return message;
        case 11:
            message = "新增失敗";
            return message;
        case 12:
            message = "修改失敗";
            return message;
        case 13:
            message = "刪除失敗";
            return message;
        default:
    }
}