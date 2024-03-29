<script setup>
import { faHome, faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import CreatedocumentDialog from "../CreatedocumentDialog.vue";
import CreatefolderDialog from "../CreatefolderDialog.vue";
import CreateTagDialog from "../CreatetagDialog.vue";
import axios from "../../axios";
</script>

<template>
  <div class="sideBar">
    <div class="items">
      <RouterLink v-for="item in sidebarItems" :key="item.name" :to="item.link">
        <font-awesome-icon :icon="item.icon" class="icon" />
      </RouterLink>
    </div>
    <div class="create-icons">
      <div class="create-folder-icon" @click="openCreateFolderDialog">
        <i class="fa-solid fa-folder-plus fa-2x"></i>
      </div>
      <div class="create-icon" @click="openCreateDocumentDialog">
        <i class="fa-solid fa-file-circle-plus fa-2x"></i>
      </div>
      <div class="create-tag-icon" @click="openCreateTagDialog">
        <i class="fa-solid fa-tags fa-2x"></i>
      </div>
    </div>
    <div class="bottom">
      <div class="logout-icon" @click="logout">
        <i class="fa-solid fa-right-from-bracket fa-2x"></i>
      </div>
      <div class="logo">
        <img src="@/assets/logo.png" />
      </div>
    </div>
  </div>
  <createdocument-dialog
    ref="createDocumentDialog"
    @document-created="createDocument"
    @clear-validation="clearValidation"
    :folders="folders"
    :validation="validation"
  ></createdocument-dialog>
  <CreatefolderDialog
    ref="createFolderDialog"
    @folder-created="createFolder"
    @clear-validation="clearValidation"
    :validation="validation"
  ></CreatefolderDialog>
  <CreateTagDialog
    ref="createTagDialog"
    @tag-created="createTag"
    @clear-validation="clearValidation"
    :validation="validation"
  ></CreateTagDialog>
</template>

<script>
export default {
  data() {
    return {
      folders: [],
      validation: {},
      sidebarItems: [
        { name: "Home", icon: faHome, link: "/" },
        { name: "About", icon: faInfoCircle, link: "/about" },
      ],
    };
  },
  components: {
    CreatedocumentDialog,
    CreatefolderDialog,
  },
  created() {
    this.fetchFolders();
  },
  methods: {
    logout() {
      localStorage.removeItem("jwtToken");
      location.reload();
    },
    clearValidation() {
      this.validation = {};
    },
    async openCreateFolderDialog() {
      this.$refs.createFolderDialog.open();
    },
    async openCreateDocumentDialog() {
      this.$refs.createDocumentDialog.open();
    },
    async openCreateTagDialog() {
      this.$refs.createTagDialog.open();
    },
    async createFolder(createdFolder) {
      try {
        const titles = await this.fetchDocuments(createdFolder.DocumentTitles);
        const payload = {
          name: createdFolder.name,
          DocumentTitles: titles,
        };
        await axios.post("/folders", payload);
        this.emitter.emit("created-folder-2", payload, true);
        this.$refs.createFolderDialog.close();
        this.fetchFolders();
      } catch (e) {
        if (e.response) {
          this.validation = Object.keys(e.response.data.errors).reduce(
            (prev, key) => {
              const newKey = key.charAt(0).toLowerCase() + key.slice(1);
              prev[newKey] = e.response.data.errors[key][0];
              return prev;
            },
            {}
          );
        } else {
          this.validation = "Error when creating folder";
        }
      }
    },
    fetchDocuments(DocumentTitles) {
      try {
        if (!DocumentTitles) {
          return [];
        }
        const documentTitlesArray = DocumentTitles.split(",");
        DocumentTitles = documentTitlesArray.map((title) => title.trim());
        return documentTitlesArray;
      } catch (e) {
        alert("Error fetching document titles");
      }
    },
    async createDocument(createdDocument) {
      try {
        const tagIds = await this.fetchTags(createdDocument.tags);
        const payload = {
          title: createdDocument.title,
          content: createdDocument.content ?? "",
          type: createdDocument.type,
          tags: tagIds,
        };

        const documentTitles = createdDocument.folder.documents.map(
          (doc) => doc.title
        );
        documentTitles.push(createdDocument.title);

        const folderpayload = {
          guid: createdDocument.folder.guid,
          name: createdDocument.folder.name,
          DocumentTitles: documentTitles,
        };
        await axios.post("/documents", payload);
        await axios.put(
          `folders/${createdDocument.folder.guid}`,
          folderpayload
        );
        this.emitter.emit("created-folder-2", payload, true);
        this.$refs.createDocumentDialog.close();
        this.fetchFolders();
      } catch (e) {
        if (e.response) {
          this.validation = Object.keys(e.response.data.errors).reduce(
            (prev, key) => {
              const newKey = key.charAt(0).toLowerCase() + key.slice(1);
              prev[newKey] = e.response.data.errors[key][0];
              return prev;
            },
            {}
          );
        } else {
          this.validation = "Folder is missing";
        }
      }
    },
    async createTag(createdTag) {
      try {
        const payload = {
          name: createdTag.name,
          category: createdTag.category,
        };
        console.log(createdTag);
        await axios.post("/tags", payload);
        this.$refs.createTagDialog.close();
      } catch (e) {
        if (e.response) {
          this.validation = Object.keys(e.response.data.errors).reduce(
            (prev, key) => {
              const newKey = key.charAt(0).toLowerCase() + key.slice(1);
              prev[newKey] = e.response.data.errors[key][0];
              return prev;
            },
            {}
          );
          if (Object.keys(this.validation).length === 0) {
            this.validation = { default: "Error when creating tag" };
          }
        } else {
          this.validation = { default: "Error when creating tag" };
        }
      }
    },

    async fetchTags(tags) {
      try {
        if (!tags) {
          return [];
        }
        let tagArray = [];

        if (typeof tags === "string" && tags.includes(",")) {
          tagArray = tags.split(",").map((tagName) => tagName.trim());
        } else {
          tagArray = Array.isArray(tags) ? tags : [tags];
        }
        const response = await axios.get("tags");
        const tagMap = new Map(response.data.map((tag) => [tag.name, tag.id]));
        const tagIds = tagArray.map((tagName) => ({
          tagId: tagMap.get(tagName),
        }));
        return tagIds;
      } catch (e) {
        this.validation = Object.keys(e.response.data.errors).reduce(
          (prev, key) => {
            const newKey = key.charAt(0).toLowerCase() + key.slice(1);
            prev[newKey] = e.response.data.errors[key][0];
            return prev;
          },
          {}
        );
      }
    },
    async fetchFolders() {
      try {
        const response = await axios.get("/folders");
        this.folders = response.data;
      } catch (e) {
        this.validation = "Folder is missing";
      }
    },
  },
};
</script>

<style scoped>
.create-icons {
  display: flex;
  flex-direction: column;
  gap: 2rem;
  align-items: center;
  margin-top: auto;
  padding-bottom: 1em;
}

.create-tag-icon {
  display: flex;
  flex-direction: column;
  gap: 1em;
  color: var(--icon-color);
  font-size: 1.2em;
}

.create-folder-icon {
  display: flex;
  flex-direction: column;
  gap: 1em;
  color: var(--icon-color);
  font-size: 1.2em;
}
.create-tag-icon:hover {
  color: var(--icon-hover-color);
}

.create-folder-icon:hover {
  color: var(--icon-hover-color);
}

.logout-icon {
  display: flex;
  flex-direction: column;
  gap: 1em;
  color: var(--icon-color);
  font-size: 1.2em;
  margin-top: auto;
}

.logout-icon:hover {
  color: var(--icon-hover-color);
}

.create-icon {
  display: flex;
  flex-direction: column;
  gap: 1em;
  color: var(--icon-color);
  font-size: 1.2em;
}

.create-icon:hover {
  color: var(--icon-hover-color);
}

.sideBar {
  position: fixed;
  top: 0;
  left: 0;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  background-color: var(--sidebar-background-color);
  justify-content: space-between;
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
.bottom {
  display: flex;
  flex-direction: column;
  gap: 2em;
  width: 80px;
  margin-top: auto;
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
