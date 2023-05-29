<script setup>
import axios from "axios";
import FolderCard from "../components/FolderCard.vue";
import LoadingSpinner from "../components/LoadingSpinner.vue";
</script>

<template>
  <div class="homeViewContainer">
    <LoadingSpinner v-if="loading"></LoadingSpinner>
    <h1>{{ folders.length }} Folders</h1>
    <div class="folderCards">
      <template v-for="f in folders" :key="f.guid">
        <RouterLink :to="`/folder/${f.guid}`">
          <FolderCard :folder="f"></FolderCard>
        </RouterLink>
      </template>
    </div>
  </div>
</template>

<style scoped>
.folderCards {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
}

.folderCards a {
  text-decoration: inherit;
  color: inherit;
}
</style>

<script>
export default {
  data() {
    return {
      folders: [],
      loading: false,
    };
  },
  async mounted() {
    if (this.loading) {
      return;
    }
    try {
      this.loading = true;
      this.folders = (await axios.get("folders")).data;
    } catch (e) {
      alert("Server was not reached");
    } finally {
      this.loading = false;
    }
  },
};
</script>
