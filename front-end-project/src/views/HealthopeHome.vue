<template>
  <div >
    <BtnNormal text="新增帳號" @click="goAddMember"></BtnNormal>
    <BtnNormal text="登出" @click="logout"></BtnNormal>
  </div>
</template>

<script>
import BtnNormal from "@/components/Btn/BtnNormal";

export default {
  name: "HealthopeHome",
  components: {
    BtnNormal,
  },
  data() {
    return {
    };
  },
  methods:{
    goAddMember(){
      this.$router.push("/member/add")
    },
     async logout() {
      const response = await this.$axios.post("/api/AccountAccess/MemberLogout");

      if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
        this.unwatchFlag = this.$watch(
          "notificationBoxConfirmFlag",
          (newVal) => {
            if (newVal) {
              let redirectRoute = "stop";
              this.$emit("afterConfirmEvent", redirectRoute);
              this.unwatchFlag(); // 移除監聽
              this.unwatchFlag = null;
            }
          }
        );

        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "登出成功!";
        this.$notificationBox.notificationBoxErrorCode =0
        return;
      } else {
        // 添加監聽器，查看彈窗是否被按確認鍵
        this.unwatchFlag = this.$watch(
          "notificationBoxConfirmFlag",
          (newVal) => {
            if (newVal) {
              let redirectRoute = "/";
              this.$emit("afterConfirmEvent", redirectRoute);
              this.unwatchFlag(); // 移除監聽
              this.unwatchFlag = null;
            }
          }
        );

        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "發生錯誤!";
        this.$notificationBox.notificationBoxErrorCode =
          response.data.ErrorCode;
      }
    },
  }
};
</script>

<style scoped>

</style>