<template>
  <div class="aboutViewContainer">
    <h1>About</h1>
    <p></p>
    <h4>Made By:</h4>
    <div class="Collaborators">
      <div class="name">Tristan Losada Benini</div>
      <div v-on:click="copyEmail"><i class="fas fa-envelope"></i></div>
    </div>
    <p>(in collaboration with Ruben Osmanovic)</p>
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
      copiedText: '',
    };
  },
  methods: {
    copyEmail() {
      const email = 'LOS20421@spengergasse.at';
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
</style>