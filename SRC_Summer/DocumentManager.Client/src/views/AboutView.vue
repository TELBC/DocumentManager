<template>
  <div class="aboutViewContainer">
    <h1>About</h1>
    <p></p>
    <h4>Made By:</h4>
    <div class="name">Tristan Losada Benini</div>
    <div v-on:click="copyEmail('LOS20421@spengergasse.at')">
      <i class="fas fa-envelope"></i>
    </div>
    <p>(in collaboration with Ruben Osmanovic)</p>
    <img id="spengerlogo" src="@/assets/spengerlogo.svg" />
    <div v-if="showPopup" class="popup">
      Copied to clipboard: {{ copiedText }}
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      showPopup: false,
      copiedText: "",
    };
  },
  methods: {
    copyEmail(email) {
      navigator.clipboard.writeText(email).then(() => {
        this.copiedText = email;
        this.showPopup = true;
        setTimeout(() => {
          this.showPopup = false;
        }, 2000);
      });
    },
  },
};
</script>

<style>
.popup {
  position: fixed;
  top: 10px;
  right: 10px;
  background-color: #f0f0f0;
  padding: 10px;
  border-radius: 5px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
}

#spengerlogo {
  margin-top: 2rem;  
  height: 30px;
}
</style>
