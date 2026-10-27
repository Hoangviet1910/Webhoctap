---
name: mvc-learning-ui
description: Safely audits and redesigns the UI of an existing ASP.NET Core MVC learning project using a dark futuristic neon design system and rich animations while preserving backend logic, database, routes, authentication, validation, and existing functionality.
---

# MVC Learning UI Skill

## Purpose

Redesign the user interface of an existing ASP.NET Core MVC learning application with a premium dark futuristic, neon technology aesthetic and rich visual animations.

## Mandatory safety rules

1. Inspect the actual repository structure before proposing or changing files.
2. Never assume a Controller, View, route, Model, or feature exists without verifying it.
3. Preserve the database, schema, records, Entity Framework configuration, migrations, and DbContext.
4. Preserve Controller actions, routes, HTTP methods, business logic, authentication, authorization, and API contracts.
5. Preserve form names, IDs, tag helpers, model binding, validation, anti-forgery tokens, and CRUD behavior.
6. Do not remove existing features to simplify the redesign.
7. Do not install packages, run migrations, delete files, or execute destructive commands without explicit user approval.
8. Do not overwrite original assets or configuration without approval.
9. Inspect the Git status and existing changes before any implementation.
10. Never claim a test passed unless it was actually run and its result was verified.

## Design direction

- Dark futuristic visual language.
- Deep charcoal and navy backgrounds.
- Cyan, electric blue, purple, and magenta accents.
- Controlled glow, glassmorphism, layered depth, and refined borders.
- Rich but purposeful transitions, hover states, reveal effects, and micro-interactions.
- Strong visual hierarchy focused on learning tasks and progress.
- Responsive layouts for desktop, tablet, and mobile.
- Accessible contrast, keyboard navigation, visible focus, and reduced-motion support.

## Workflow

### Phase 1: Audit only

- Read applicable project instructions and relevant UI skills.
- Identify real Controllers, actions, Views, layouts, partials, stylesheets, scripts, and assets.
- Map each page to its actual files and existing behavior.
- Record uncertainties and potential regression risks.
- Do not edit or create files during an audit-only request.

### Phase 2: Design proposal

- Propose design tokens, typography, spacing, components, page layouts, and animation rules.
- Link every proposed page redesign to verified existing files.
- Separate verified findings from assumptions.
- List the files expected to change and explain why.
- Wait for explicit user approval before implementation.

### Phase 3: Incremental implementation

- Implement only the approved scope.
- Prefer existing Views, CSS, and JavaScript where practical.
- Avoid unnecessary dependencies and large animation libraries.
- Preserve all backend contracts and existing behavior.
- Work in small, reviewable changes.
- Test affected pages, forms, validation, routing, authentication, and CRUD flows.
- Report changed files, tests performed, and unresolved issues.

## Animation and performance

- Use animation to improve hierarchy and feedback, not to obstruct learning.
- Prefer transform and opacity for frequent motion.
- Avoid unnecessary continuous animations and expensive effects.
- Respect prefers-reduced-motion.
- Provide graceful behavior on mobile and lower-powered devices.
- Do not add 3D or particle effects without evaluating their performance cost.

## Reporting

For each audit or implementation, clearly state:

- Verified files and behavior.
- Proposed or completed changes.
- Risks and safeguards.
- Tests actually performed.
- Remaining uncertainties.

Never start implementation when the user has requested audit or proposal only.