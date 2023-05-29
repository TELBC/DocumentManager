<template>
  <div class="folderCard">
    <div class="icon">
      <i v-if="folder.documents.length > 0" class="fas fa-folder fa-5x"></i>
      <i v-else class="far fa-folder-open fa-5x"></i>
      <div class="count">{{ folder.documents.length }}</div>
    </div>
    <div class="folderName">{{ folder.name }}</div>
  </div>
</template>

<style scoped>

.icon {
  color: var(--icon-color);
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 110px;
}
.icon:hover{
  color: var(--icon-hover-color);
}

.count {
  position: absolute;
  font-size: 80%;
  font-weight: 900;
  top: 55px;
  background-color: white;
  border-radius: 50%;
  width: 20px;
  height: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
}


.folderCard {
  width: 120px;
  min-height: 130px;
  border-radius: 5px;
  display: flex;
  flex-direction: column;
  padding: 0.5rem;
}


.folderCard:hover {
  background-color: aliceblue;
}

.folderName {
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 120%;
  flex-grow: 1;
}
</style>

<script>
export default {
  props: {
    folder: Object,
  },
};
</script>