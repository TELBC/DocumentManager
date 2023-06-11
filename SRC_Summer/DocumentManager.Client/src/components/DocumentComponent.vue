<script setup>
import axios from "../axios";
import ConfirmDialog from "./ConfirmDialog.vue";
import EditDocumentDialog from "./EditdocumentDialog.vue";
</script>

<template>
  <div class="document-container">
    <div class="document">
      <div class="documentsHeader" @click="loadContent(document.guid)">
        <div class="title" :title="document.title">{{ document.title }}</div>
        <div class="version" :title="document.version" :style="versionStyle">
          v.{{ document.version }}
        </div>
        <div
          class="tags"
          v-if="document.tags.length > 0 && !show"
          @mouseover="hover = true"
          @mouseleave="hover = false"
        >
          <i class="fa-solid fa-tag tag-icon"></i>
          <div class="tag-list" v-if="hover">
            <ul>
              <li v-for="t in document.tags" :key="t">{{ t }}</li>
            </ul>
          </div>
        </div>
        <div class="type" :title="document.type">.{{ document.type }}</div>
        <div class="actions">
          <span class="loadContent">
            <i class="fa-solid fa-file"></i>
          </span>
        </div>
      </div>
      <div class="documentContent">
        <div v-show="showContent">{{ getDocument(document.guid) }}</div>
      </div>
    </div>
    <div class="document-actions">
      <button class="edit-button" @click="editDocument">Edit</button>
      <button class="delete-button" @click="confirmDelete">Delete</button>
    </div>
  </div>
  <confirm-dialog
    ref="confirmDialog"
    title="Delete Document"
    message="Are you sure you want to delete this document?"
  ></confirm-dialog>
  <edit-document-dialog
    ref="editDialog"
    :document="document"
    :guids="guids"
    :documents="documents"
    :folders="folders"
    :folder="getFolderNameByDocumentGuid(document.guid)"
    @document-updated="updateDocument"
    @clear-validation="clearValidation"
    :validation="validation"
  ></edit-document-dialog>
</template>

<script>
export default {
  props: {
    document: {
      type: Object,
      required: true,
    },
    documents: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      loading: false,
      loadedDocument: null,
      hover: false,
      showContent: false,
      guids: [],
      folders: [],
      validation: {},
    };
  },
  components: {
    ConfirmDialog,
    EditDocumentDialog,
  },
  created() {
    this.fetchFolders();
  },
  methods: {
    clearValidation() {
      this.validation = {};
    },
    getFolderNameByDocumentGuid(guid) {
      const folder = this.folders.find((folder) =>
        folder.documents.some((doc) => doc.guid === guid)
      );
      return folder ? folder.name : "";
    },
    async fetchFolders() {
      try {
        this.folders = (await axios.get("/folders")).data;
      } catch (error) {
        alert("Error fetching folders");
      }
    },
    async loadContent(documentGuid) {
      if (this.loading) {
        return;
      }
      try {
        this.loading = true;
        this.loadedDocument = (
          await axios.get(`folders/${this.folderGuid}/${documentGuid}`)
        ).data;
        this.showContent = !this.showContent;
      } catch (e) {
        alert("Server was unable to load document.");
      } finally {
        this.loading = false;
      }
    },
    getDocument(documentGuid) {
      if (this.loadedDocument && this.loadedDocument.guid === documentGuid) {
        return this.loadedDocument.content;
      }
      return null;
    },
    async confirmDelete() {
      const confirmed = await this.$refs.confirmDialog.open();
      if (confirmed) {
        try {
          await axios.delete(`documents/${this.document.guid}`);
          this.$emit("document-deleted", this.document.guid);
        } catch (error) {
          alert("Error deleting the document"); //added global error handling
        }
      }
    },
    async editDocument() {
      this.$refs.editDialog.open();
    },
    async updateDocument(updatedDocument) {
      try {
        const tagIds = updatedDocument.tags
          ? await this.fetchTags(updatedDocument.tags)
          : [];
        const payload = {
          guid: updatedDocument.guid,
          title: updatedDocument.title,
          content: updatedDocument.content,
          type: updatedDocument.type,
          tags: tagIds,
          version: updatedDocument.version + 1,
        };
        await axios.put(`documents/${updatedDocument.guid}`, payload);
        const folderResponse = await axios.get(
          `folders/${updatedDocument.folder.guid}/`
        );
        const documentTitles = folderResponse.data.documents.map(
          (doc) => doc.title
        );
        if (!documentTitles.includes(updatedDocument.title)) {
          documentTitles.push(updatedDocument.title);
        }
        const folderpayload = {
          guid: updatedDocument.folder.guid,
          name: updatedDocument.folder.name,
          DocumentTitles: documentTitles,
        };

        await axios.put(
          `folders/${updatedDocument.folder.guid}`,
          folderpayload
        );
        this.$emit("document-updated", payload, true);
        this.$refs.editDialog.close();
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
          this.validation = "Error when editing document";
        }
      }
    },
    async fetchTags(tags) {
      try {
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
        alert("Error fetching tags:", error);
      }
    },
    async fetchGuids() {
      if (this.loading) {
        return;
      }
      try {
        const response = await axios.get(`folders/${this.folderGuid}`);
        this.guids = response.data.documents.map((document) => document.guid);
      } catch (e) {
        alert("ERROR fetching guids");
      } finally {
        this.loading = false;
      }
    },
  },
  computed: {
    folderGuid() {
      return this.$route.params.id;
    },
    versionStyle() {
      return this.document.tags.length > 0 ? {} : { flexGrow: 1 };
    },
  },
  mounted() {
    this.fetchGuids();
  },
};
</script>

<style scoped>
.document-container {
  display: flex;
  gap: 0.1rem;
  align-items: center;
}

.document-actions {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
  padding: 0;
}

.document {
  background-color: #f5f5f5;
  border-radius: 10px;
  margin: 5px;
  padding: 10px;
  border: 1px solid #e0e0e0;
  width: 100%;
  width: 800px;
}

.documentsHeader {
  display: flex;
  column-gap: 1.5em;
  margin-top: 0.1rem;
  padding-bottom: 5px;
}

.documentsHeader .tags {
  flex-grow: 1;
}

.documentsHeader .actions {
  padding-right: 1rem;
  cursor: pointer;
  transition: all 0.2s;
}

.documentsHeader .actions:hover {
  color: var(--icon-hover-color);
}

.documentsHeader .title {
  flex: 0 0 10em;
  font-size: 1.1em;
  font-weight: bold;
}

.documentContent {
  padding-top: 5px;
  border-top: 1px solid #e0e0e0;
}

.documentsHeader:hover {
  background-color: #e9ecef;
}

.title,
.type,
.version {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 100%;
}

.tags {
  position: relative;
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 0.5rem;
  cursor: pointer;
  transition: all 0.2s;
}

.tag-icon {
  margin-right: 0.2rem;
}

.tag-icon:hover {
  color: var(--icon-hover-color);
}

.tag-list {
  display: none;
  position: initial;
  min-width: fit-content;
}

.tag-list ul {
  list-style-type: none;
  padding: 0;
  margin: 0;
}

.tag-list li {
  padding: 1px 0;
}

.tags:hover .tag-list {
  display: block;
}

.edit-button,
.delete-button {
  border: none;
  color: white;
  font-size: 0.8em;
  cursor: pointer;
  border-radius: 5px;
  transition: background-color 0.3s;
  padding: 0.3em;
}

.edit-button {
  background-color: var(--icon-color);
}

.edit-button:hover {
  background-color: var(--icon-hover-color);
}

.delete-button {
  background-color: var(--delete-icon-color);
}

.delete-button:hover {
  background-color: var(--delete-icon-hover-color);
}
</style>
