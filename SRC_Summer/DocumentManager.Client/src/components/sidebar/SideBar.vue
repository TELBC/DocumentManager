<script setup>
import { faHome, faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

const sidebarItems = [
  { name: "Home", icon: faHome, link: "/" },
  { name: "About", icon: faInfoCircle, link: "/about" },
];
</script>

<template>
  <div class="sideBar">
    <div class="items">
      <RouterLink v-for="item in sidebarItems" :key="item.name" :to="item.link">
        <font-awesome-icon :icon="item.icon" class="icon" />
      </RouterLink>
    </div>
    <div class="logo">
      <img src="@/assets/logo.png" />
    </div>
  </div>
</template>

<style scoped>
.sideBar {
  position: fixed;
  top: 0;
  left: 0;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  background-color: var(--sidebar-background-color);
  padding: 0.5rem 0.5rem;
  height: 100vh;
}

.items {
  padding-top: 1em;
  display: flex;
  flex-direction: column;
  gap: 2em;
  align-items: center;
  width: 80px;
}

.logo {
  display: flex;
  flex-direction: column;
  gap: 1em;
  width: 80px;
  margin-top: auto;
  padding-bottom: 1em;
}

RouterLink {
  display: flex;
  align-items: center;
  gap: 1em;
  text-decoration: none;
  color: var(--icon-color);
}

.icon {
  font-size: 2.5em;
  color: var(--icon-color);
}

.icon:hover {
  color: var(--icon-hover-color);
}
</style>
