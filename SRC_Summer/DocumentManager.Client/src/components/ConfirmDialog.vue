<!-- src/components/ConfirmDialog.vue -->
<template>
  <div
    v-if="show"
    class="confirm-dialog-overlay"
    @click="closeOnOverlayClick($event)"
  >
    <div class="confirm-dialog">
      <h2>{{ title }}</h2>
      <p>{{ message }}</p>
      <div class="confirm-dialog-actions">
        <button @click="cancel">Cancel</button>
        <button @click="confirm">Confirm</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    title: String,
    message: String,
  },
  data() {
    return {
      show: false,
      resolve: null,
    };
  },
  methods: {
    closeOnOverlayClick(event) {
      if (event.target === event.currentTarget) {
        this.cancel();
      }
    },
    open() {
      this.show = true;
      return new Promise((resolve) => {
        this.resolve = resolve;
      });
    },
    close() {
      this.show = false;
    },
    confirm() {
      this.resolve(true);
      this.close();
    },
    cancel() {
      this.resolve(false);
      this.close();
    },
  },
};
</script>

<style scoped>
.confirm-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.confirm-dialog {
  background-color: #f0f0f0;
  padding: 20px;
  border-radius: 5px;
}

.confirm-dialog-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

button {
  background-color: red;
  border: none;
  color: white;
  padding: 0.5em 1em;
  font-size: 1em;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
}

button:hover {
  background-color: darkred;
}

button:first-child {
  background-color: #6c757d;
}

button:first-child:hover {
  background-color: #5a6268;
}
</style>
