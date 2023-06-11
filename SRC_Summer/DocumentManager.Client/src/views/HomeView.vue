<script setup>
import axios from "axios";
import FolderCard from "../components/FolderCard.vue";
import LoadingSpinner from "../components/LoadingSpinner.vue";
import ConfirmDialog from "../components/ConfirmDialog.vue";
</script>

<template>
  <div class="homeViewContainer">
    <LoadingSpinner v-if="loading"></LoadingSpinner>
    <h1>{{ folders.length }} Folders</h1>
    <div class="folderCards">
      <template v-for="f in folders" :key="f.guid">
        <div>
          <RouterLink :to="`/folder/${f.guid}`">
            <div class="folderCardContainer">
              <FolderCard :folder="f"></FolderCard>
            </div>
          </RouterLink>
          <div class="delete-button-container">
            <button class="delete-button" @click="confirmDelete(f.guid)">
              Delete
            </button>
          </div>
        </div>
      </template>
    </div>
  </div>
  <confirm-dialog
    ref="confirmDialog"
    title="Delete Folder"
    message="Are you sure you want to delete this folder?"
  ></confirm-dialog>
</template>

<script>
export default {
  created() {
    this.emitter.on("created-folder-2", this.loadFolders);
  },
  data() {
    return {
      folders: [],
      loading: false,
    };
  },
  methods: {
    async confirmDelete(guid) {
      const confirmed = await this.$refs.confirmDialog.open();
      if (confirmed) {
        try {
          await axios.delete(`folders/${guid}`);
          this.folders = this.folders.filter((f) => f.guid !== guid);
        } catch (e) {
          alert("Error deleting the folder"); //added global error handling
        }
      }
    },
    async loadFolders() {
      this.loading = true;
      try {
        this.folders = (await axios.get(`folders`)).data;
      } catch (e) {
        alert("Server was unable to load folder."); //added global error handling
      } finally {
        this.loading = false;
      }
    },
  },

  async mounted() {
    if (this.loading) {
      return;
    }
    try {
      this.loading = true;
      this.folders = (await axios.get("folders")).data;
    } catch (e) {
      alert("Server was not reached"); //added global error handling
    } finally {
      this.loading = false;
    }
  },
};
</script>

<style scoped>
.folderCardContainer {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.folderCards {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
}

.folderCards a {
  text-decoration: inherit;
  color: inherit;
}

.delete-button-container {
  display: flex;
  justify-content: center;
}

.delete-button {
  border: none;
  color: white;
  font-size: 0.8em;
  cursor: pointer;
  border-radius: 5px;
  transition: background-color 0.3s;
  padding: 0.3em;
  margin-top: 0.5em;
}

.delete-button {
  background-color: var(--delete-icon-color);
}

.delete-button:hover {
  background-color: var(--delete-icon-hover-color);
}
</style>
