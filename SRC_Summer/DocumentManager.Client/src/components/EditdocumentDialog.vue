<template>
  <div
    v-if="show"
    class="edit-dialog-overlay"
    @click="closeOnOverlayClick($event)"
  >
    <div class="edit-dialog">
      <h2>Edit Document</h2>
      <form @submit.prevent="submit">
        <label for="guid">GUID:</label>
        <select
          id="guid"
          v-model="editedDocument.guid"
          class="custom-select"
          @change="updateContent"
        >
          <option v-for="guid in guids" :key="guid" :value="guid">
            {{ guid }}
          </option>
        </select>

        <label for="title">Title:</label>
        <input type="text" id="title" v-model="editedDocument.title" />

        <label for="content">Content:</label>
        <textarea id="content" v-model="editedDocument.content"></textarea>

        <label for="tags">Tags:</label>
        <input type="text" id="tags" v-model="editedDocument.tags" />

        <label for="version">Version:</label>
        <input type="text" id="version" v-model="editedDocument.version" />

        <div class="edit-dialog-actions">
          <button type="button" @click="cancel">Cancel</button>
          <button type="submit">Save</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    document: {
      type: Object,
      required: true,
    },
    guids: {
      type: Array,
      required: true,
    },
    documents: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      show: false,
      editedDocument: {},
    };
  },

  watch: {
    "editedDocument.guid": {
      immediate: true,
      handler(newGuid) {
        const documentWithGuid = this.documents.find(
          (doc) => doc.guid === newGuid
        );
        if (documentWithGuid) {
          this.editedDocument.title = documentWithGuid.title;
          this.editedDocument.content = documentWithGuid.content;
          this.editedDocument.tags = documentWithGuid.tags;
          this.editedDocument.version = documentWithGuid.version;
        }
      },
    },
  },

  methods: {
    open() {
      this.editedDocument = { ...this.document };
      this.show = true;
    },
    close() {
      this.show = false;
    },
    closeOnOverlayClick(event) {
      if (event.target === event.currentTarget) {
        this.close();
      }
    },
    cancel() {
      this.close();
    },
    submit() {
      this.$emit("document-updated", { ...this.editedDocument });
      this.close();
    },
    async updateContent() {
      const documentWithGuid = this.documents.find(
        (doc) => doc.guid === this.editedDocument.guid
      );
      if (documentWithGuid) {
        this.editedDocument.content = documentWithGuid.content;
      }
    },
  },
};
</script>

<style scoped>
.edit-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.edit-dialog {
  background-color: white;
  border-radius: 10px;
  padding: 30px;
  max-width: 550px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1), 0 1px 3px rgba(0, 0, 0, 0.08);
  display: flex;
  flex-direction: column;
}

h2 {
  font-weight: bold;
  font-size: 24px;
  margin-bottom: 20px;
}

label {
  font-weight: bold;
  font-size: 14px;
  margin-bottom: 5px;
}

input,
textarea {
  font-size: 14px;
  padding: 5px 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  margin-bottom: 15px;
  width: 96%; /* somehow this fixes the fields ¯\_(ツ)_/¯*/
}

textarea {
  height: 200px;
  overflow: auto;
  resize: none;
}

.edit-dialog-actions {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1em;
}

.custom-select {
  font-size: 14px;
  padding: 5px 30px 5px 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  margin-bottom: 15px;
  width: 100%;
  appearance: none;
  background: url("../assets/caret-down-solid.svg") no-repeat;
  background-position: right 10px center;
  background-size: 0.65em auto;
}

button {
  background-color: var(--icon-color);
  border: none;
  color: white;
  padding: 0.5em 1em;
  font-size: 1em;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
}

button:hover {
  background-color: var(--icon-hover-color);
}

button:first-child {
  background-color: var(--cancel-color);
}

button:first-child:hover {
  background-color: var(--cancel-hover-color);
}
</style>
