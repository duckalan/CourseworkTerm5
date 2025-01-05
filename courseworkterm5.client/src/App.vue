<script setup lang="ts">
import { ref, onBeforeMount } from 'vue';
import type { Ref } from 'vue';
import { RouterView } from 'vue-router'
import SiteHeader from '@/components/SiteHeader.vue'

</script>

<template>
  <SiteHeader />

  <RouterView />

  <footer>

  </footer>
</template>
