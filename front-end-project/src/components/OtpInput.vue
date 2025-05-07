<template>
  <div>
    <SubTitleCard text="簡訊驗證"></SubTitleCard>
    <div class="otpBox" v-if="step === 1">
      <div class="otpContainer">
        <div>
          <label class="label">請輸入驗證碼：</label>
          <div class="otpInputContainer">
            <input v-model="otpCode" />
            <BtnNormal
              :text="
                cooldownFlag ? `請等待 ${remainingSecond} 秒` : '重新發送 OTP'
              "
              :disabled="cooldownFlag"
              @click="sendOtp"
            ></BtnNormal>
          </div>
        </div>
      </div>
      <div class="hintContainer">
        <span v-if="invalidInput" class="hintSpan">{{ this.hintText }}</span>
      </div>
      <div class="btnContainer">
        <BtnConfirm @click="verifyOtp" text="簡訊驗證"></BtnConfirm>
      </div>
    </div>
    <div class="successBox" v-if="step === 2">
      <div>
        <h2>✅ 驗證成功</h2>
      </div>
      <div class="successBtnContainer">
        <BtnConfirm text="返回主頁" @click="redirect('/')"></BtnConfirm>
      </div>
    </div>
  </div>
</template>

<script>
import BtnNormal from "@/components/Btn/BtnNormal";
import BtnConfirm from "@/components/Btn/BtnConfirm";
import SubTitleCard from "@/components/Card/SubTitleCard";

export default {
  name: "HealthopeAddMember",
  components: {
    BtnConfirm,
    BtnNormal,
    SubTitleCard,
  },
  data() {
    return {
      hintText: "",
      invalidInput: false,
      otpCode: "",
      remainingSecond: 0,
      step: 1,
    };
  },
  methods: {
    async sendOtp() {
      try {
        // 剩餘冷卻時間>0 =>回傳
        if (this.cooldownFlag) return;
        // 假設呼叫的是這個 API，會回傳類似 { RemainingSecond: 180 ( 單位是 s ) }
        const response = await this.$axios.post(
          "/api/Member/GetOtpAtVerifyPhone"
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          const remainingSecond = response.data.ApiDataObject.RemainingSecond;

          if (remainingSecond > 0) this.startCooldown(remainingSecond);
        } else if (
          response.data.ErrorCode === this.$errorCodeDefine.AlreadyVerify
        ) {
          this.step = 2;
          return;
        } 
        else if(response.data.ErrorCode === this.$errorCodeDefine.OtpCooldown){
        this.invalidInput = true;
        this.hintText = "距上次發送 OTP 未達 3 分鐘";
          const remainingSecond = response.data.ApiDataObject.RemainingSecond;
          if (remainingSecond > 0) this.startCooldown(remainingSecond);
        }
        else {
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
          return;
        }
      } catch (error) {
        console.error("發送 OTP 時發生錯誤：", error);
      }
    },
    async verifyOtp() {
      if (this.otpCode.length !== 6) {
        this.invalidInput = true;
        this.otpCode = "";
        this.hintText = "請輸入 6 位數字驗證碼";
        return;
      }

      if (isNaN(this.otpCode.trim())) {
        this.invalidInput = true;
        this.otpCode = "";
        this.hintText = "請輸入 6 位數字驗證碼";
        return;
      }

      this.invalidInput = false;

      try {
        // post 參數
        let verifyPhoneDto = {
          OtpCode: this.otpCode,
        };

        // 假設呼叫的是這個 API，會回傳類似 { RemainingSecond: 180 ( 單位是 s ) }
        const response = await this.$axios.post(
          "/api/Member/VerifyPhone",
          verifyPhoneDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.step = 2;
          const remainingSecond = response.data.ApiDataObject.RemainingSecond;
          if (remainingSecond > 0) this.startCooldown(remainingSecond);
          return;
        } else if (
          response.data.ErrorCode === this.$errorCodeDefine.AlreadyVerify
        ) {
          this.step = 2;
          return;
        } else if (
          response.data.ErrorCode === this.$errorCodeDefine.VerifyFail
        ) {
          this.invalidInput = true;
          this.otpCode = "";

          // OTP 過時
          if (response.data.ApiDataObject.RemainingSecond == 0)
            this.hintText = "OTP 已過時，請重新發送";
          else this.hintText = "驗證失敗";

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
        console.error("發送 OTP 時發生錯誤：", error);
      }
    },
    startCooldown(seconds) {
      if (this.timer) {
        clearInterval(this.timer);
      }

      this.remainingSecond = seconds;
      this.timer = setInterval(() => {
        this.remainingSecond--;
        if (this.remainingSecond <= 0) {
          clearInterval(this.timer);
          this.timer = null;
        }
      }, 1000);
    },
    redirect(path) {
      this.$router.push(path);
    },
  },
  created() {
    this.sendOtp();
  },
  computed: {
    cooldownFlag() {
      return this.remainingSecond > 0;
    },
  },
};
</script>

<style scoped>
.otpBox {
  width: 100%;
}

.hintContainer,
.btnContainer {
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  margin-top: 15px;
}

.otpContainer {
  width: 100%;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  margin-top: 25px;
  margin-left: 25px;
}

.otpInputContainer {
  display: flex;
  flex-wrap: wrap;
  margin-top: 8px;
  gap: 0.5rem;
  width: 400px;
}

.otpInputContainer input {
  border-radius: 0.5rem;
  padding: 0.7rem 0.75rem;
  border: none;
  width: 50%;
  background-color: white;
  outline: 2px solid #efefef;
  font-size: 15px;
}

.otpInputContainer input:focus {
  outline: 2px solid #707070;
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