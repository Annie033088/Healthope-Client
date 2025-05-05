import VueRouter from "vue-router";
// import Vue from 'vue';
import HealthopeHome from '@/views/HealthopeHome';
import HealthopeAddMember from '@/views/Member/HealthopeAddMember';
// import axios from 'axios';
// import { errorCodeDefine } from '../utils/globalSetting';

const routes = [
    {
        path: '/',
        name: 'HealthopeHome',
        component: HealthopeHome,
        meta: { requireAuth: null } 
    },
    {
        path: '/member/add',
        name: 'HealthopeAddMember',
        component: HealthopeAddMember,
        meta: { requireAuth: null } 
    },
    {
        path: '*',
        name: 'HealthopeDefault',
        component: HealthopeHome,
        meta: { requireAuth: null }
    }
]

const router = new VueRouter({
    routes,
    mode: 'history'
});

// router.beforeEach(async (to, from, next) => {
//     const requireAuth = to.meta.requireAuth;
//     let havePermissionDto;

//     if (requireAuth === "login" || null) {
//         havePermissionDto = null
//     } else {
//         havePermissionDto = requireAuth
//     }

//     const response = await axios.post("/api/AccountAccess/HavePermission", havePermissionDto);

//     // 如果使用者未登入
//     if (response.data.ErrorCode === errorCodeDefine.UserNotLogin) {
//         if (to.name === 'HealthopeLogin') {
//             Vue.prototype.$loginFlag = false;
//             return next();
//         }
//         Vue.prototype.$loginFlag = false;
//         return next({ name: 'HealthopeLogin' });
//     } else {
//         Vue.prototype.$loginFlag = true;
//     }

//     // 如果使用者已經登入，不讓他進入登入頁，直接導到首頁
//     if (to.name === 'HealthopeLogin' && response.data.ErrorCode !== errorCodeDefine.UserNotLogin) {
//         Vue.prototype.$loginFlag = true;
//         return next({ name: 'HealthopeHome' });
//     }

//     // 有權限且有登入
//     if (response.data.ErrorCode === errorCodeDefine.Success) {
//         Vue.prototype.$loginFlag = true;
//         return next();
//     }
//     // 其他情況只剩沒權限, 轉倒到主頁
//     else {
//         return next({ name: 'HealthopeHome' });
//     }
// });

export default router