import Vue from 'vue';
export default function (mock) {
    mock.onPost("/api/AccountAccess/AddMember").reply(config => {
        const addMemberDto = JSON.parse(config.data);

        if (addMemberDto) {
            const regex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;

            if (regex.test(addMemberDto.Account) && regex.test(addMemberDto.Pwd) &&
                addMemberDto.Account !== addMemberDto.Pwd) {
                Vue.prototype.$loginFlag = true;
                return [200, { ErrorCode: 1 }];
            } else {
                return [200, { ErrorCode: 10 }];
            }
        }
    })
    mock.onPost("/api/AccountAccess/AdminLogout").reply(() => {
        let errorCode = 1
        if (errorCode === 1)
            Vue.prototype.$loginFlag = false;

        return [200, { ErrorCode: errorCode }]
    })
}