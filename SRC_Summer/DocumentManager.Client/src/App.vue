<script setup>
import SideBar from "./components/sidebar/SideBar.vue";
</script>

<template>
  <div id="appContainer">
    <SideBar></SideBar>
    <main>
      <router-view></router-view>
    </main>
  </div>
</template>

<style scoped>
#appContainer {
  font-family: "Inconsolata";
  display: flex;
}

main {
  font-family: "Inconsolata";
  padding-left: 13vh;
}
</style>
