export default function (mock) {
    mock.onPost("/api/Member/AddMember").reply(config => {
        const addMemberDto = JSON.parse(config.data);

        if(addMemberDto){
            const regex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;

            if (regex.test(addMemberDto.Account) && regex.test(addMemberDto.Pwd) &&
            addMemberDto.Account !== addMemberDto.Pwd) {
                return [200, { ErrorCode: 1 }];
            } else {
                return [200, { ErrorCode: 10 }];
            }
        }
    })
}