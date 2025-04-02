<template>
  <div class="flex h-screen bg-gray-100">
    <!-- Sidebar -->
    <sidebar-view />
    <router-view />
  </div>
</template>

<script setup>
import SidebarView from "../components/SidebarView.vue";
</script>

<style>
.el-button--primary {
  background-color: #958be8 !important;
  border-color: #958be8 !important;
}
</style>
