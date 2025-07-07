<template>
  <div class="addContainer">
    <SubTitleCard text="帳號註冊"></SubTitleCard>
    <div class="inputBox" v-if="step === 1">
      <div class="inputContainer">
        <InputSpan
          class="inputSpan"
          labelText="帳號"
          v-model="account"
          :required="true"
        ></InputSpan>
        <InputSpan
          class="inputSpan"
          labelText="手機號碼"
          v-model="phone"
          :required="true"
        ></InputSpan>
        <InputSpan
          class="inputSpan"
          labelText="信箱"
          v-model="email"
          :required="false"
        ></InputSpan>
        <InputSpan
          class="inputSpan"
          labelText="密碼"
          v-model="pwd"
          inputType="password"
          :required="true"
        ></InputSpan>
        <InputSpan
          class="inputSpan"
          labelText="再輸入一次密碼"
          v-model="pwdAgain"
          inputType="password"
          :required="true"
        ></InputSpan>
      </div>
      <div class="hintContainer">
        <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
      </div>
      <div class="btnContainer">
        <BtnConfirm @click="addMember" text="加入會員"></BtnConfirm>
      </div>
    </div>
    <div class="successBox" v-if="step === 2">
      <div>
        <h2>✅ 註冊成功</h2>
      </div>
      <div class="successBtnContainer">
        <BtnConfirm text="手機簡訊驗證" @click="goSendOtp"></BtnConfirm>
      </div>
    </div>
  </div>
</template>


<script>
import InputSpan from "@/components/Input/InputSpan";
import BtnConfirm from "@/components/Btn/BtnConfirm";
import SubTitleCard from "@/components/Card/SubTitleCard";

export default {
  name: "HealthopeAddMember",
  components: {
    InputSpan,
    BtnConfirm,
    SubTitleCard,
  },
  props: {
    text: String,
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      hintText: "",
      account: "",
      pwd: "",
      pwdAgain: "",
      phone: "",
      email: "",
      verifyFail: false,
      step: 1,
    };
  },
  methods: {
    async goSendOtp() {
      this.$router.push("/member/phoneVerification");
    },
    async addMember() {
      // 帳號密碼驗證用的正規表達式 ( 8~20 位英數字)
      const accountPwdRegex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;
      this.account = this.account.trim();
      this.phone = this.phone.trim();
      this.email = this.email.trim();
      this.pwd = this.pwd.trim();
      this.pwdAgain = this.pwdAgain.trim();

      if (
        !(accountPwdRegex.test(this.account) && accountPwdRegex.test(this.pwd))
      ) {
        this.hintText = "請輸入 8~20 位英文數字";
        this.verifyFail = true;
        return;
      }

      // 手機驗證用的正規表達式，僅限台灣 ( 09 開頭，後面8碼 )
      let phone = Number(this.phone);
      const phoneRegex = /^[9]\d{8}$/;

      if (!phoneRegex.test(phone)) {
        this.hintText = "請輸入正確的手機號碼（例如：09xxxxxxxx）";
        this.verifyFail = true;
        return;
      }

      if (this.account === this.pwd) {
        this.hintText = "帳號密碼不可相同";
        this.verifyFail = true;
        return;
      }

      if (this.pwd !== this.pwdAgain) {
        this.hintText = "兩次密碼輸入不一致";
        this.verifyFail = true;
        return;
      }

      if (!this.validEmail(this.email)) {
        this.hintText = "信箱錯誤";
        this.verifyFail = true;
        return;
      }

      try {
        // 傳輸資料
        const addMemberDto = {
          Account: this.account,
          Phone: phone,
          Email: this.email,
          Pwd: this.pwd,
        };

        // post後回傳
        const response = await this.$axios.post(
          "/api/AccountAccess/AddMember",
          addMemberDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.verifyFail = false;
          this.step = 2;
          return;
        } else if (
          response.data.ErrorCode === this.$errorCodeDefine.DuplicatePhone
        ) {
          this.hintText = "註冊的手機號碼重複";
          this.verifyFail = true;
          return;
        } else if (
          response.data.ErrorCode === this.$errorCodeDefine.DuplicateAccount
        ) {
          this.hintText = "註冊的帳號重複";
          this.verifyFail = true;
          return;
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = null;
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
      } catch (error) {
        console.error("創建管理者時發生錯誤", error);
      }
    },
    validEmail(email) {
      // 可空
      if (!email) return true;

      // [^\s@] 代表至少一個不是空白或 @ 的字元
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // EX: abc@ewq.ee
      if (email.length > 254) return false; // 規定總長最長 254

      const parts = email.split("@");
      if (parts.length !== 2) return false;

      const [localPart, domain] = parts;

      if (
        localPart.length < 3 || // 建議最少 3 字元
        localPart.length > 64 || // 規定 @以前 最長 64
        domain.length > 251 // 不得超過 254 - 3
      ) {
        return false;
      }

      return emailRegex.test(email);
    },
  },
};
</script>

<style scoped>
.addContainer {
  width: 100%;
}

.inputContainer {
  width: 60%;
  max-width: 350px;
}

.inputBox {
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  margin-top: 25px;
  justify-content: center;
}

.inputSpan {
  margin-top: 3%;
}

.hintContainer,
.btnContainer {
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  margin-top: 15px;
  justify-content: center;
}

.hintSpan {
  color: #c07878;
  animation: slideInTop 0.5s cubic-bezier(0.25, 0.46, 0.45, 0.94) both;
}

@keyframes slideInTop {
  0% {
    transform: translateY(-30px);
    opacity: 0;
  }
  100% {
    transform: translateY(0);
    opacity: 1;
  }
}

.successBox {
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  flex-direction: column;
  margin-top: 25px;
}
.successBox h2 {
  display: flex;
  justify-content: center;
}

.successBtnContainer {
  width: 100%;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  margin-top: 15px;
  margin-left: 15px;
}
</style>