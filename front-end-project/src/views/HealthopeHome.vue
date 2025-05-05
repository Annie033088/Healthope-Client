<template>
  <div >
    <BtnNormal text="新增帳號" @click="goAddMember"></BtnNormal>
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
    }
  }
};
</script>

<style scoped>

</style>