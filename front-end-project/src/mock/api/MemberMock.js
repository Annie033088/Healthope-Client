export default function (mock) {
    mock.onPost("/api/Member/GetOtpAtVerifyPhone").reply(() => {
        return [200, { ErrorCode: 1, ApiDataObject:{RemainingSecond: 180} }];
    })
    mock.onPost("/api/Member/VerifyPhone").reply(() => {
        return [200, { ErrorCode: 1, ApiDataObject:{RemainingSecond: 180}}];
    })
}