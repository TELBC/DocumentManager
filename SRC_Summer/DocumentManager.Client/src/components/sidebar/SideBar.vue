<script setup>
import { faHome, faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import CreatedocumentDialog from "../CreatedocumentDialog.vue";
import CreatefolderDialog from "../CreatefolderDialog.vue";
import axios from "axios";
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
    </div>
    <div class="logo">
      <img src="@/assets/logo.png" />
    </div>
  </div>
  <createdocument-dialog
    ref="createDocumentDialog"
    @document-created="createDocument"
    :folders="folders"
  ></createdocument-dialog>
  <CreatefolderDialog
    ref="createFolderDialog"
    @folder-created="createFolder"
  ></CreatefolderDialog>
</template>

<script>
const sidebarItems = [
  { name: "Home", icon: faHome, link: "/" },
  { name: "About", icon: faInfoCircle, link: "/about" },
];

export default {
  data() {
    return {
      folders: [],
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
    async openCreateFolderDialog() {
      this.$refs.createFolderDialog.open();
    },
    async openCreateDocumentDialog() {
      this.$refs.createDocumentDialog.open();
    },
    async createFolder(createdFolder) {
      try {
        const titles = await this.fetchDocuments(createdFolder.DocumentTitles);
        const payload = {
          name: createdFolder.name,
          DocumentTitles: titles,
        };
        await axios.post("/folders", payload);
        this.emitter.emit("created-folder-2", payload);
        this.fetchFolders();
      } catch (error) {
        alert("Error creating folder"); //added global error handling
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
      } catch (error) {
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
        this.emitter.emit("created-folder-2", payload);
        this.fetchFolders();
      } catch (error) {
        alert("Error creating document"); //added global error handling
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
      } catch (error) {
        alert("Error fetching tags"); //added global error handling
      }
    },
    async fetchFolders() {
      try {
        const response = await axios.get("/folders");
        this.folders = response.data;
      } catch (error) {
        alert("Error fetching folders");
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

.create-folder-icon {
  display: flex;
  flex-direction: column;
  gap: 1em;
  color: var(--icon-color);
  font-size: 1.2em;
}

.create-folder-icon:hover {
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
